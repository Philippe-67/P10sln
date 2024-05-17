using System.ComponentModel.DataAnnotations;

namespace MSUi.Models.Authentification
{
    public class Login
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
