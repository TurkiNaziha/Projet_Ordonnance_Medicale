using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.DTO;
using WebApplication1.Models.Repository;
using WebApplication1.DTOs;
using Azure;

public class DemandeRepository : IDemandeRepository
{
	private readonly DsContext _context;

	public DemandeRepository(DsContext context)
    {
        _context = context;
    }

   // public async Task<List<DemandeDTO>> GetAllAsync()
   // {
   //     var demandes = await _context.Demandes
   //         .Include(d => d.Medicaments)
   //         .Include(d => d.Patient)
   //         .ThenInclude(p => p.Medecin) // Assuming Medecin navigation property exists
   //         .ToListAsync();

   //     return demandes.Select(d => new DemandeDTO
   //     {
   //         Id = d.Id,
   //         PatientName = d.Patient?.Nom + " "+d.Patient?.Prenom, // Replace "Name" with the actual property for Patient name
   //         Medicaments = d.Medicaments.Select(m => new MedicamentDTO
   //         {
   //             ID = m.Id,
   //             Nom = m.Nom,
   //             Label = m.Label,
   //             Prix = m.Prix,
   //             Quantite = m.Quantite
   //         }).ToList(),
			////Montant = d.Medicaments.Sum(m => m.Prix * m.Quantite),
			//Etat = d.Etat,
   //         Date = d.Date
   //     }).ToList();
   // }

    public async Task<DemandeDTO> GetByIdPatientAsync(int id)
    {
        var demande = await _context.Demandes
            .Include(d => d.Medicaments)
            .Include(d => d.Patient)
            .ThenInclude(p => p.Medecin) // Assuming Medecin navigation property exists
            .FirstOrDefaultAsync(d => d.PatID == id);

        if (demande == null) return null;

        return new DemandeDTO
        {
            Id = demande.Id,
            PatientName = demande.Patient?.Nom + " " + demande.Patient?.Prenom, // Replace "Name" with the actual property for Patient name
            Medicaments = demande.Medicaments.Select(m => new MedicamentDTO
            {
                ID = m.ID,
                Nom = m.Nom,
                Label = m.Label,
                Prix = m.Prix,
                Quantite = m.Quantite
            }).ToList(),
			//Montant = demande.Medicaments.Sum(m => m.Prix * m.Quantite),
			Etat = demande.Etat,
            Date = demande.Date
        };
    }

	//public async Task AddAsync(DemandeDTO demandeDTO)
	//{
	//	// Create a new Demand object
	//	var demande = new Demand
	//	{
	//		// Set properties based on the DemandeDTO
	//		Ordonnance = demandeDTO.Ordonnance,
	//		Date = demandeDTO.Date,
	//		Etat = Demand.State.NotApproved, // Default value for Etat is NotApproved
	//		Montant = 0, // Default value for Montant is 0
	//		PatID = int.TryParse(demandeDTO.PatientID, out int patID) ? patID : 0, // Assuming PatientID is a string in DemandeDTO, convert it to an integer
	//		Medicaments = new List<Medicament>() // Empty list for Medicaments
	//	};

	//	// You may need to add logic here to handle Medicaments if necessary, 
	//	// but since you mentioned an empty list, we are not processing it here.

	//	// Insert the Demand into the database
	//	_context.Demandes.Add(demande);

	//	// Save changes asynchronously
	//	await _context.SaveChangesAsync();
	//}

	public async Task UpdateAsync(Demand demande)
    {
        var existingDemand = await _context.Demandes.FindAsync(demande.Id);
        if (existingDemand != null)
        {
            _context.Entry(existingDemand).CurrentValues.SetValues(demande);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var demande = await _context.Demandes.FindAsync(id);
        if (demande != null)
        {
            _context.Demandes.Remove(demande);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ChangeStateAsync(int demandeId, Demand.State etat)
    {
        var demande = await _context.Demandes.FindAsync(demandeId);
        if (demande != null)
        {
            demande.Etat = etat;
            await _context.SaveChangesAsync();
        }
    }

	public async Task ChangeMTAsync(int demandeId, decimal montant)
	{
		Demand demande = await _context.Demandes.FindAsync(demandeId);
		if (demande != null)
		{
			demande.Montant = montant;
			await _context.SaveChangesAsync();
		}
	}



	public async Task ChangeMedicAsync(int demandeId, List<MedicamentDTO> medicaments)
    {
        var demande = await _context.Demandes
            .Include(d => d.Medicaments)
            .FirstOrDefaultAsync(d => d.Id == demandeId);

        if (demande != null)
        {
            // Clear existing Medicaments
            demande.Medicaments.Clear();

            // Add updated Medicaments
            foreach (var medicament in medicaments)
            {
                demande.Medicaments.Add(medicament);
            }

            await _context.SaveChangesAsync();
        }
    }

	public async Task AddAsync(Demand demand)
	{
		// Add the demand to the database context
		await _context.Demandes.AddAsync(demand);

		// Save changes to persist the new demand in the database
		await _context.SaveChangesAsync();
	}


	public async Task<List<DemandeDTO>> GetAllAsync()
    {
        // Query all the demands from the database with necessary relationships
        var demands = await _context.Demandes
            .Include(d => d.Patient)           // Include related Patient
            .ThenInclude(p => p.Medecin)       // Include related Medecin through Patient
            .ToListAsync();

        // Map each Demand to a DemandeDTO
        var demandeDTOs = demands.Select(d => new DemandeDTO
        {
            Id = d.Id,
            Ordonnance = d.Ordonnance,
            PatientName = d.Patient?.Nom ?? "",                  // Handle null Patient
            PatientID = d.PatID.ToString(),                     // Convert PatID (assumed integer) to string
            MedecinName = d.Patient?.Medecin?.Nom ?? "",  
            Montant=d.Montant,
			Medicaments = d.Medicaments,// Handle null Medecin

			Date = d.Date,
            Etat = d.Etat,                                       // Map Etat to DemandeDTO (adjust as needed)
                                                                 // Medicaments mapping can be added here if necessary
        }).ToList();

        return demandeDTOs; // Return the mapped list
    }


	public async Task<List<DemandeDTO>> GetAllByPatientNameAsync(string patientName)
	{
		// Query all the demands from the database, filtering by patient name
		var demands = await _context.Demandes
			.Include(d => d.Patient)           // Include related Patient
			.ThenInclude(p => p.Medecin)       // Include related Medecin through Patient
			.Where(d => d.Patient != null && d.Patient.Nom.Contains(patientName)) // Filter by Patient's name
			.ToListAsync();

		// Map each Demand to a DemandeDTO
		var demandeDTOs = demands.Select(d => new DemandeDTO
		{
			Id = d.Id,
			Ordonnance = d.Ordonnance,
			PatientName = d.Patient?.Nom ?? "",
			PatientID = d.PatID.ToString(),
			MedecinName = d.Patient?.Medecin?.Nom ?? "",
			Montant = d.Montant,
			Medicaments = d.Medicaments, // Assuming Medicaments is a collection or related data
			Date = d.Date,
			Etat = d.Etat,
		}).ToList();

		return demandeDTOs; // Return the mapped list
	}

	public async Task<List<DemandeDTO>> GetAllByMedecinNameAsync(string medecinName)
	{
		// Query all the demands from the database, filtering by medecin name
		var demands = await _context.Demandes
			.Include(d => d.Patient)           // Include related Patient
			.ThenInclude(p => p.Medecin)       // Include related Medecin through Patient
			.Where(d => d.Patient != null && d.Patient.Medecin != null && d.Patient.Medecin.Nom.Contains(medecinName)) // Filter by Medecin's name
			.ToListAsync();

		// Map each Demand to a DemandeDTO
		var demandeDTOs = demands.Select(d => new DemandeDTO
		{
			Id = d.Id,
			Ordonnance = d.Ordonnance,
			PatientName = d.Patient?.Nom ?? "",
			PatientID = d.PatID.ToString(),
			MedecinName = d.Patient?.Medecin?.Nom ?? "",
			Montant = d.Montant,
			Medicaments = d.Medicaments, // Assuming Medicaments is a collection or related data
			Date = d.Date,
			Etat = d.Etat,
		}).ToList();

		return demandeDTOs; // Return the mapped list
	}

	Task<Demand> IDemandeRepository.GetByIdPatientAsync(int id)
	{
		throw new NotImplementedException();
	}

	//Task IDemandeRepository.AddAsync(Demand demande)
	//{
	//	throw new NotImplementedException();
	//}

	Task IDemandeRepository.UpdateAsync(Demand demande)
	{
		throw new NotImplementedException();
	}

	//public async Task DeleteAsync(int id)
	//{
	//	// Find the demand by its ID
	//	var demand = await _context.Demandes.FindAsync(id);

	//	// If the demand is not found, throw an exception or handle accordingly
	//	if (demand == null)
	//	{
	//		throw new KeyNotFoundException($"Demand with ID {id} was not found.");
	//	}

	//	// Remove the demand from the database context
	//	_context.Demandes.Remove(demand);

	//	// Save changes to apply the deletion
	//	await _context.SaveChangesAsync();
	//}


	



	public async Task<Demand> GetByIdAsync(int id)
	{
		return await _context.Demandes
			.Include(d => d.Medicaments) // Include related Medicaments if needed
			.FirstOrDefaultAsync(d => d.Id == id);
	}

}


