using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;


namespace SkripsiIvan.Models
{

    public partial class Gejala
    {
        public Gejala()
        {
            RekamMedik = new HashSet<RekamMedik>();
            Rule = new HashSet<Rule>();
        }

        public int IdGejala { get; set; }
        public string NamaGejala { get; set; }
        public int? ParentId { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
        public double? Interval { get; set; }
        public int KategoriId { get; set; }
        public string GambarGejala { get; set; }
        public virtual ICollection<RekamMedik> RekamMedik { get; set; }
        public virtual ICollection<Rule> Rule { get; set; }
        public virtual Kategori Kategori { get; set; }
        [NotMapped]
        public IFormFile FileGambar { get; set; }

    }
}
