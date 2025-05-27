namespace WebApplication1.DTO
{
	public class MedicamentDTO
	{
		public int ID { get; set; }
		public string Nom { get; set; } // Name of the medication
		public string Label { get; set; } // Description or label of the medication
		public int Quantite { get; set; } // Quantity of the medication
		public decimal Prix { get; set; } // Price of a single unit of the medication
	}
}
