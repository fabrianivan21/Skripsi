using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SkripsiIvan.Models
{
    public partial class PenyakitViewModels
    { 

        public int IdPenyakit { get; set; }
        [Required]
        public string NamaPenyakit { get; set; }
        public string Solusi { get; set; }
        public int KategoriId { get; set; }
        public List<Penyakit> Penyakits { get; set; }
    }
}
