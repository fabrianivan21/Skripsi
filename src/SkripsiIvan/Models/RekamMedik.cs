using System;
using System.Collections.Generic;

namespace SkripsiIvan.Models
{
    public partial class RekamMedik
    {
        public int IdRekammedik { get; set; }
        public int IdPenyakit { get; set; }
        public int IdGejala { get; set; }
        public string Kode { get; set; }
        public int KategoriId { get; set; }

        public virtual Gejala IdGejalaNavigation { get; set; }
        public virtual Penyakit IdPenyakitNavigation { get; set; }
        public virtual Kategori Kategori { get; set; }
    }
}
