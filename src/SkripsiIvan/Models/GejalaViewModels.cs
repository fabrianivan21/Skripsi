using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkripsiIvan.Models
{
    public partial class GejalaViewModels
    {
        public int IdGejala { get; set; }
        public string NamaGejala { get; set; }
        public int? ParentId { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
        public double? Interval { get; set; }
        public string MinMax { get; set; }
        public int KategoriId { get; set; }
        public string GambarGejala { get; set; }
        
        public List<Gejala> Gejalas { get; set; }
        [NotMapped]
        public IFormFile FileGambar { get; set; }

    }
}
