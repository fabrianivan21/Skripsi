using System;
using System.Collections.Generic;

namespace SkripsiIvan.Models
{
    public partial class Admin
    {
        public int IdAdmin { get; set; }
        public string Name { get; set; }
        public string UsernameAdmin { get; set; }
        public string PasswordAdmin { get; set; }
    }
}
