using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
	public class PatientDTO
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[MaxLength(100)]
		public string Nom { get; set; }

		[Required]
		[MaxLength(100)]
		public string Prenom { get; set; }

		[Required]
		[EmailAddress]
		[MaxLength(150)]
		public string Mail { get; set; }

		[Required]
		[Phone]
		[MaxLength(15)]
		public string Telephone { get; set; }

		[Required]
		[MaxLength(150)]
		public string Maladie { get; set; }

		public int MedecinID { get; set; }
		public string MedecinName { get; set; }

		// Optional: Include Medecin details if needed in response
	}
}
