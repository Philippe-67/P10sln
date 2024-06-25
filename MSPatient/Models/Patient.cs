using System.ComponentModel.DataAnnotations;

namespace MSPatient.Models
{
    //public class Patient
    //{
    //    public int Id { get; set; }
    //    [Required(ErrorMessage = "le prénom est requis")]
    //    public string Prenom { get; set; }
    //    [Required(ErrorMessage = "le nom est requis")]
    //    public string Nom { get; set; }

    //    [DataType(DataType.Date, ErrorMessage = "Veuillez entrer une date de naissance valide.")]
    //    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public DateTime DateDeNaissance { get; set; }
    //    [Required(ErrorMessage = "Le genre est requis")]
    //    public string Genre { get; set; }
    //    public string ? Adresse { get; set; }
    //    public string ? Tel { get; set; } 
    //}
    public class Patient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le prénom est requis")]
        [MaxLength(50, ErrorMessage = "La longueur maximale du prénom est de 50 caractères")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [MaxLength(50, ErrorMessage = "La longueur maximale du nom est de 50 caractères")]
        public string Nom { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Veuillez entrer une date de naissance valide.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateDeNaissance { get; set; }

        [Required(ErrorMessage = "Le genre est requis")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Le genre doit être un caractère (F ou M)")]
        public string Genre { get; set; }

        public string? Adresse { get; set; }

        public string? Tel { get; set; }
    }
}
