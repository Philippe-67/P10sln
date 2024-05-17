﻿using System.ComponentModel.DataAnnotations;

namespace MSUi.Models.Authentification
{
    public class Register
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }

        public string Password { get; set; }
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
