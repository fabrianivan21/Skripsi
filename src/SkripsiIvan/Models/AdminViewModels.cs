using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SkripsiIvan.Models
{
    public class AdminViewModels
    {
        public int IdAdmin { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string UsernameAdmin { get; set; }
        [Required]
        public string PasswordAdmin { get; set; }

        public string AddError { get; set; }

        public List<Admin> Users { get; set; }
    }

    public class UsersEditViewModels
    {
        public int IdUsers { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
