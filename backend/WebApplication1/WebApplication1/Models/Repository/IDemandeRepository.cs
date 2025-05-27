using Microsoft.EntityFrameworkCore;
using WebApplication1.DTO;
using WebApplication1.DTOs;

namespace WebApplication1.Models.Repository
{
	public interface IDemandeRepository
	{
		//Task<List<Demand>> GetAllAsync();
		Task<List<DemandeDTO>> GetAllAsync();
		Task<Demand> GetByIdPatientAsync(int id);

		Task<Demand> GetByIdAsync(int id);

		Task AddAsync(Demand demande);
		Task UpdateAsync(Demand demande);
		Task DeleteAsync(int id);

		//Task<Medecin> GetMedecinByIdAsync(int id);
		//Task<Patient> GetPatientByIdAsync(int id);

		// Méthodes modifiées pour correspondre à la signature dans le repository
		Task ChangeStateAsync(int demandeId, Demand.State etat);
		Task ChangeMTAsync(int demandeId, decimal montant);
		Task ChangeMedicAsync(int demandeId, List<MedicamentDTO> medicaments);
		Task<List<DemandeDTO>> GetAllByPatientNameAsync(string patientName);
		Task<List<DemandeDTO>> GetAllByMedecinNameAsync(string medecinName);
		
	}
}
