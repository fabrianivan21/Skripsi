using System;
using System.Collections.Generic;

namespace SkripsiIvan.Models
{
    public class RekamMedikString
    {
        public int IdRekamMedik { get; set; }
        public string Penyakit { get; set; }
        public string Gejala { get; set; }
        public string Kategori { get; set; }
        public int? GroupId { get; set; }
        public string Kode { get; set; }
        
    }
}
