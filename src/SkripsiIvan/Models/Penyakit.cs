using System;
using System.Collections.Generic;

namespace SkripsiIvan.Models
{
    public partial class Penyakit
    {
        public Penyakit()
        {
            RekamMedik = new HashSet<RekamMedik>();
            Rule = new HashSet<Rule>();
        }

        public int IdPenyakit { get; set; }
        public string NamaPenyakit { get; set; }
        public string Solusi { get; set; }
        public int KategoriId { get; set; }

        public virtual ICollection<RekamMedik> RekamMedik { get; set; }
        public virtual ICollection<Rule> Rule { get; set; }
        public virtual Kategori Kategori { get; set; }
    }
}
