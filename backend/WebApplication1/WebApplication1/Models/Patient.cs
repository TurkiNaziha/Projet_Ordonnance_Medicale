using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
	public class Patient
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
		[EmailAddress]
		[MaxLength(150)]
		public string Maladie { get; set; }


		[Required]
		public int MedecinID { get; set; }

		[ForeignKey("MedecinID")]
		public virtual Medecin Medecin { get; set; }

		public virtual ICollection<Demand> Demandes { get; set; }
	}
}
