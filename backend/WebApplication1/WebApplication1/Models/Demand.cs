using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.DTO;

namespace WebApplication1.Models
{
	public class Demand
	{
		public enum State
		{
			NotApproved,
			Approved,
			Done
		}

		[Key]
		public int Id { get; set; }


		[Required]
		[MaxLength(255)]
		public string? Ordonnance { get; set; }

		[Required]
		public DateTime Date { get; set; } = DateTime.Now;

		
		public State Etat { get; set; } = State.NotApproved;

		
		
		public decimal Montant { get; set; }

	

		//public List<Medicament> Medicaments { get; set; }


		[Required]
		public int PatID { get; set; }

		[ForeignKey("PatID")]
		public virtual Patient Patient { get; set; }

		
		public  List<MedicamentDTO> Medicaments { get; set; }  // Liste navigable


	}

}