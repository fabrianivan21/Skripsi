using System;
using System.Collections.Generic;

namespace SkripsiIvan.Models
{
    public partial class Rule
    {
        public int IdRule { get; set; }
        public int IdPenyakit { get; set; }
        public int IdGejala { get; set; }
        public int? Total { get; set; }
        public int KategoriId { get; set; }

        public virtual Gejala IdGejalaNavigation { get; set; }
        public virtual Penyakit IdPenyakitNavigation { get; set; }
        public virtual Kategori Kategori { get; set; }
    }
}
