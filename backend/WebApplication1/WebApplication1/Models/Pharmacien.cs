using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Pharmacien
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)] // Longueur maximale pour le nom
        public string Nom { get; set; }

        [Required]
        [MaxLength(100)] // Longueur maximale pour le prénom
        public string Prenom { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)] // Longueur maximale pour l'email
        public string Email { get; set; }
    }
}
