using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Medecin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)] // Adjust the length as needed
        public string Nom { get; set; }

        [Required]
        [MaxLength(100)] // Adjust the length as needed
        public string Prenom { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)] // Adjust the length as needed
        public string Mail { get; set; }

        [Required]
        [Phone]
        [MaxLength(15)] // Adjust based on expected phone number length
        public string Telephone { get; set; }

        [Required]
        [MaxLength(100)] // Adjust the length as needed
        public string Specialite { get; set; }

		public virtual ICollection<Patient> Patients { get; set; }
	}
}
