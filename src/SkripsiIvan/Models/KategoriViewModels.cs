using System;
using System.Collections.Generic;

namespace SkripsiIvan.Models
{
    public partial class KategoriViewModels
    {
        public int KategoriPenyakit { get; set; }
        public string NamaKategori { get; set; }
        public List<Kategori> Kategoris { get; set; }
    }
}
