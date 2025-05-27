using System;
using System.Collections.Generic;
using WebApplication1.DTO;
using static WebApplication1.Models.Demand;

namespace WebApplication1.DTOs
{
	public class DemandeDTO
	{
		public int Id { get; set; } // Id of the demand
		public string? Ordonnance { get; set; }
		public string PatientName { get; set; } // Full name of the patient
		public string PatientID { get; set; }
		public string MedecinName { get; set; } // Full name of the medecin associated with the patient

		public List<MedicamentDTO> Medicaments { get; set; } = new List<MedicamentDTO>(); // List of medications

		public decimal Montant { get; set; }
		

		public State Etat { get; set; } // State of the demand (NotApproved, Approved, Done)

		public DateTime Date { get; set; } // Date of the demand
	}


}
