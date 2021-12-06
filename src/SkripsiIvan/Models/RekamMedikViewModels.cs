using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SkripsiIvan.Models
{
    public partial class RekamMedikViewModels
    {
        public int IdMedik { get; set; }
        [Required]
        public int IdPenyakit { get; set; }
        public int IdGejala { get; set; }
        public int KategoriId { get; set; }
        public int? GroupId { get; set; }
        //[Required]
        public string Kode { get; set; }
        [Required]
        public string ListGejala { get; set; }
        public int Metode { get; set; }
        public virtual Gejala IdGejalaNavigation { get; set; }
        public virtual Penyakit IdPenyakitNavigation { get; set; }

        public List<RekamMedikString> RekamMediks { get; set; }
    }
}
