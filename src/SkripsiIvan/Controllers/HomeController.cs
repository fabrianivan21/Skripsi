using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkripsiIvan.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SkripsiIvan.Controllers
{
    public static class ExceptionHelper
    {
        public static int LineNumber(this Exception e)
        {

            int linenum = 0;
            try
            {
                //linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(":line") + 5));

                //For Localized Visual Studio ... In other languages stack trace  doesn't end with ":Line 12"
                linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(' ')));

            }


            catch
            {
                //Stack trace is not available!
            }
            return linenum;
        }
    }
    public class HomeController : Controller
    {
        private readonly Models.SkripsiIvanContext db;

        public HomeController(Models.SkripsiIvanContext _db)
        {
            db = _db;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Konsultasi()
        {
            return View();
        }

        public JsonResult Konsuls(string gejala, int metode, int KategoriId)
        {
            try
            {
                List<Tuple<string, float>> HasilDiagnosa = new List<Tuple<string, float>>();
                List<Tuple<float, string, string, string>> Hasil = new List<Tuple<float, string, string, string>>();
                List<Tuple<int, float>> Gejala = gejala.Split(',').Select(x => new Tuple<int, float>(Convert.ToInt32(x.Split('-').FirstOrDefault()), float.Parse(x.Split('-').Last(), System.Globalization.CultureInfo.InvariantCulture))).ToList();
                //if(Gejala.Count < 3)
                //{
                //   return Json("Anda belum memilih minimal 3 gejala!");
                //}
                //foreach(var item in Gejala) {
                //    Console.WriteLine(gejala);
                var rules = db.Rule.Where(x => x.KategoriId == KategoriId).Include(x => x.IdPenyakitNavigation).Include(x => x.IdGejalaNavigation).ToList();
                var Penyakit = rules.Where(x => Gejala.Select(y => y.Item1).Contains(x.IdGejala)).GroupBy(x => x.IdPenyakit).ToList();
                List<Tuple<int, int, int>> terpicu = new List<Tuple<int, int, int>>();

                Penyakit.ForEach(x => terpicu.Add(new Tuple<int, int, int>(x.Key, rules.Where(q => q.IdPenyakit == x.Key).Count(y => Gejala.Select(z => z.Item1).Contains(y.IdGejala)), rules.Where(q => q.IdPenyakit == x.Key).Count() - rules.Where(q => q.IdPenyakit == x.Key).Count(y => Gejala.Select(z => z.Item1).Contains(y.IdGejala)))));
                var kotak2 = "";
                var kotak3 = "";
                List<Tuple<int, float, string>> data3 = new List<Tuple<int, float, string>>();

                foreach (var p in terpicu.OrderBy(x => x.Item3))//.OrderByDescending(x => x.Item2))
                {
                    List<Tuple<float, float, int>> data = new List<Tuple<float, float, int>>();

                    foreach (var item in Penyakit.FirstOrDefault(x => x.Key == p.Item1).Where(x => Gejala.Select(y => y.Item1).Contains(x.IdGejala)).Select(x => x.IdGejala))
                    {
                        var g = Gejala.FirstOrDefault(x => x.Item1 == item);
                        Console.WriteLine(g);
                        float i;
                        var interval = g.Item2;
                        var min = MinMax(g.Item2).FirstOrDefault().Key;
                        var max = MinMax(g.Item2).FirstOrDefault().Value;
                        if (metode == 0)
                        {
                            if (interval <= min || interval >= max)
                            {
                                i = 0;
                            }
                            else if (interval >= min && interval <= (float)(((max - min) / 2) + min))
                            {
                                i = (float)((interval - min) / ((float)(((max - min) / 2))));
                            }
                            else //if (interval >= (float)(max / 2) && interval <= (max)
                            {
                                i = (float)((max - interval) / (max - (float)(((max - min) / 2) + min)));
                            }
                        }
                        else
                        {

                            if (interval <= min || interval >= max)
                            {
                                i = 0;
                            }
                            else if (interval >= min + 0.1 && interval <= min + 0.1)
                            {
                                i = (float)((interval - min) / ((float)(min + 0.1) - min));
                            }
                            else if (interval >= min + 0.1 && interval <= max - 0.1)
                            {
                                i = 1;
                            }
                            else //if (interval >= (float)(max / 2) && interval <= (max)
                            {
                                i = (float)((max - interval) / (max - (float)(max - 0.01)));
                            }
                        }
                        data.Add(new Tuple<float, float, int>((float)interval, i, item));
                        Console.WriteLine(data);
                    }
                    float top = 0;
                    float down = 0;
                    foreach (var item in data)
                    {
                        top += item.Item2 * item.Item1;
                        down += item.Item2;
                    }
                    var hasil = (top / down) * 100;

                    var parah = "";
                    if (hasil <= 25)
                    {
                        parah = "Ringan";
                    }
                    else if (hasil <= 50)
                    {
                        parah = "Agak Parah";
                    }
                    else if (hasil <= 75)
                    {
                        parah = "Parah";
                    }
                    else if (hasil > 75)
                    {
                        parah = "Sangat Parah";
                    }

                    if (db.Penyakit.FirstOrDefault(x => x.IdPenyakit == p.Item1).NamaPenyakit != "Katarak" && hasil > 50)
                    {
                        Penyakit.RemoveAll(x => x.Key == p.Item1);
                    }
                    else if (db.Penyakit.FirstOrDefault(x => x.IdPenyakit == p.Item1).NamaPenyakit != "Katarak Akut" && hasil < 26 && hasil > 75)
                    {
                        Penyakit.RemoveAll(x => x.Key == p.Item1);
                    }
                    else if (db.Penyakit.FirstOrDefault(x => x.IdPenyakit == p.Item1).NamaPenyakit != "Tidak diperlukan operasi katarak" && hasil > 25)
                    {
                        Penyakit.RemoveAll(x => x.Key == p.Item1);
                    }
                    else if (db.Penyakit.FirstOrDefault(x => x.IdPenyakit == p.Item1).NamaPenyakit != "Miopi" && hasil > 50)
                    {
                        Penyakit.RemoveAll(x => x.Key == p.Item1);
                    }
                    else if(db.Penyakit.FirstOrDefault(x => x.IdPenyakit == p.Item1).NamaPenyakit != "Miopi Akut" && hasil < 26 && hasil > 75)
                    {
                        Penyakit.RemoveAll(x => x.Key == p.Item1);
                    }
                    else if (db.Penyakit.FirstOrDefault(x => x.IdPenyakit == p.Item1).NamaPenyakit == "Kegagalan Operasi Mata" && hasil < 75)
                    {
                        Penyakit.RemoveAll(x => x.Key == p.Item1);
                    }
                    else if (db.Penyakit.FirstOrDefault(x => x.IdPenyakit == p.Item1).NamaPenyakit != "Miopi Akut" && hasil < 26 && hasil > 75)
                    {
                        Penyakit.RemoveAll(x => x.Key == p.Item1);
                    }
                    else
                    {

                    }


                    kotak2 += "<span>Penyakit yang memiliki gejala yang hampir mirip adalah  " + db.Penyakit.FirstOrDefault(x => x.IdPenyakit == p.Item1).NamaPenyakit + "<br/> Gejala : " + String.Join(", ", rules.Where(x => x.IdPenyakit == p.Item1 && Gejala.Select(y => y.Item1).Contains(x.IdGejala)).Select(x => x.IdGejalaNavigation.NamaGejala).ToArray()) + "</span><hr />";// +"(" + data.Where(y => y.Item3 == x.IdGejala).FirstOrDefault().Item2 + ")"
                    data3.Add(new Tuple<int, float, string>(p.Item1, hasil, "<span>" + db.Penyakit.FirstOrDefault(x => x.IdPenyakit == p.Item1).NamaPenyakit + " dengan tingkat keparahan " + Math.Round(hasil, 2).ToString() + "% " + parah + "</span><br />"));
                     
                }
                var SortedHasil = data3.OrderByDescending(x => x.Item2).ToList();
                SortedHasil.ForEach(x => kotak3 += x.Item3);
                // data3.ForEach(x => kotak3 += x.Item3);
                var kotak1 = "";
                var kotak4 = "<span>" + db.Penyakit.FirstOrDefault(x => x.IdPenyakit == data3.OrderByDescending(y => y.Item2).FirstOrDefault().Item1).Solusi + "</span>";
                Gejala.ForEach(x => kotak1 += "<span>" + db.Gejala.FirstOrDefault(y => y.IdGejala == x.Item1).NamaGejala + "</span><br />");
                return Json(new { error = false, data1 = kotak1, data2 = kotak2, data3 = kotak3, data4 = kotak4 });
            }
            catch (Exception ex)
            {
                return Json(new { error = true, errorMsg = ex.Message });
            }
        }

        public Dictionary<float, float> MinMax(float i)
        {

            var data = new Dictionary<float, float>();
            if (i >= 0 && i <= 0.4)
            {
                data.Add(0, 0.4F);
            }
            else if (i >= 0.3 && i <= 0.7)
            {
                data.Add(0.3F, 0.7F);
            }
            else if (i >= 0.6 && i <= 1)
            {
                data.Add(0.6F, 1);
            }
            return data;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModels model)
        {

            if (ModelState.IsValid)
            {
                if (db.Admin.Any(x => x.UsernameAdmin == model.Username.Trim().ToUpper() && AdminController.VerifyHashedPassword(x.PasswordAdmin, model.Password)))
                {
                    var users = db.Admin.Single(x => x.UsernameAdmin == model.Username.Trim().ToUpper());
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, users.UsernameAdmin) };
                    var userIdentity = new ClaimsIdentity(claims, "masuk");

                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    //HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(userIdentity));

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("", "Admin");
                }
                model.Password = "";
                ModelState.AddModelError("error", "Username atau Password tidak cocok. Harap diperiksa lagi!");
            }
            model.Password = "";
            return View(model);
        }

        public async Task<IActionResult> Keluar()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Bantuan()
        {
            ViewData["Message"] = "Halaman Bantuan Anda";
            return View(await db.Gejala.Include(x => x.Kategori).ToListAsync());
        }

        public IActionResult Kontak()
        {
            ViewData["Message"] = "Halaman Kontak";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
