namespace WebApplication1.Models
{

	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Medicament
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(100)] // Adjust the length as needed
		public string Nom { get; set; }

		[Required]
		[MaxLength(200)] // Adjust the length as needed
		public string Label { get; set; }

		[Required]
		public decimal Prix { get; set; }

		[Required]
		[MaxLength(255)] // Adjust the length for the image path
		public string Image { get; set; }

		[Required]
		public int Quantite { get; set; }


		//[Required]
		//public int DemandeID { get; set; }

		//[ForeignKey("DemandeID")]
		//public virtual Demand Demande { get; set; }
	}

}
