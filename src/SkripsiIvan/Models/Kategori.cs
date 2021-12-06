using System;
using System.Collections.Generic;

namespace SkripsiIvan.Models
{
    public partial class Kategori
    {
        public Kategori()
        {
            Gejala = new HashSet<Gejala>();
            Penyakit = new HashSet<Penyakit>();
            RekamMedik = new HashSet<RekamMedik>();
            Rule = new HashSet<Rule>();
            Rule = new HashSet<Rule>();
        }

        public int KategoriPenyakit { get; set; }
        public string NamaKategori { get; set; }

        public virtual ICollection<Gejala> Gejala { get; set; }
        public virtual ICollection<Penyakit> Penyakit { get; set; }
        public virtual ICollection<RekamMedik> RekamMedik { get; set; }
        public virtual ICollection<Rule> Rule { get; set; }
    }
}
