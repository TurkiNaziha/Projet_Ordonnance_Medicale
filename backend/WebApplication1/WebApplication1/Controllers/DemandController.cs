using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.DTO;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Models.Repository;

[ApiController]
[Route("api/[controller]")]
public class DemandController : ControllerBase
{
	private readonly IDemandeRepository _demandeRepository;
	private readonly IPatientRepository patientRepository;

	private readonly DsContext _context;

	public DemandController(IDemandeRepository demandeRepository, DsContext context, IPatientRepository patientRepository)
	{
		_demandeRepository = demandeRepository;
		_context = context;
		this.patientRepository = patientRepository;
	}

	[HttpGet]
	public async Task<ActionResult<List<DemandeDTO>>> GetAll()
	{
		// Call the repository method
		var demandeDTOs = await _demandeRepository.GetAllAsync();

		// Return the list of DemandeDTOs wrapped in an Ok result
		return Ok(demandeDTOs);
	}


	[HttpGet("{id}")]
	public async Task<ActionResult<DemandeDTO>> GetById(int id)
	{
		var demande = await _demandeRepository.GetByIdPatientAsync(id);
		if (demande == null)
		{
			return NotFound();
		}

		return Ok(demande);
	}
	public enum State
	{
		NotApproved,
		Approved,
		Done
	}
	[HttpPost]
	public async Task<ActionResult> Add([FromBody] DemandeDTO demandeDTO)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		// Map DemandeDTO to Demand entity
		var demand = new Demand
		{
			Ordonnance = demandeDTO.Ordonnance,
			Date = demandeDTO.Date,
			Etat = WebApplication1.Models.Demand.State.NotApproved, // Ensure correct State enum
			Montant = 0,
			PatID = int.TryParse(demandeDTO.PatientID, out var patId) ? patId : 0, // Safe conversion
			Medicaments = new List<MedicamentDTO>() // Empty list
		};

		// Add the demand using the repository
		await _demandeRepository.AddAsync(demand);

		// Return a Created response
		return CreatedAtAction(nameof(GetById), new { id = demand.Id }, demandeDTO);
	}


	[HttpGet("by-patient/{patientName}")]
	public async Task<IActionResult> GetAllByPatientName(string patientName)
	{
		if (string.IsNullOrEmpty(patientName))
		{
			return BadRequest("Patient name cannot be empty.");
		}

		var demands = await _demandeRepository.GetAllByPatientNameAsync(patientName);
		return Ok(demands);  // Return the filtered list
	}

	// Get all demands by medecin name
	[HttpGet("by-medecin/{medecinName}")]
	public async Task<IActionResult> GetAllByMedecinName(string medecinName)
	{
		if (string.IsNullOrEmpty(medecinName))
		{
			return BadRequest("Medecin name cannot be empty.");
		}

		var demands = await _demandeRepository.GetAllByMedecinNameAsync(medecinName);
		return Ok(demands);  // Return the filtered list
	}


	[HttpPut("{id}")]
	public async Task<ActionResult> Update(int id, [FromBody] DemandeDTO demandeDTO)
	{
		// Validate the incoming model
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		// Retrieve the existing Demand entity
		var existingDemand = await _demandeRepository.GetByIdAsync(id);
		if (existingDemand == null)
		{
			return NotFound($"Demand with ID {id} not found.");
		}

		// Update the properties of the existing Demand entity
		existingDemand.Ordonnance = demandeDTO.Ordonnance;
		existingDemand.Date = demandeDTO.Date;
		existingDemand.Etat = WebApplication1.Models.Demand.State.Approved; // Assuming 'Approved' is the updated state
		existingDemand.PatID = int.TryParse(demandeDTO.PatientID, out var patId) ? patId : existingDemand.PatID;

		// Update the Medicaments only if provided in the DTO
		if (demandeDTO.Medicaments != null && demandeDTO.Medicaments.Any())
		{
			//existingDemand.Medicaments = demandeDTO.Medicaments.Select(m => new Medicament
			//{
			//	Id = m.ID,
			//	Nom = m.Nom,
			//	Label = m.Label,
			//	Quantite = m.Quantite,
			//	Prix = m.Prix
			//}).ToList();
		}

		// Save changes via the repository
		await _demandeRepository.UpdateAsync(existingDemand);

		// Return a NoContent response to indicate success without returning data
		return NoContent();
	}


	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(int id)
	{
		await _demandeRepository.DeleteAsync(id);
		return NoContent();
	}

	[HttpPatch("ChangeState/{id}")]
	public async Task<ActionResult> ChangeState(int id, [FromQuery] Demand.State state)
	{
		await _demandeRepository.ChangeStateAsync(id, state);
		return NoContent();
	}

	[HttpPatch("ChangeMontant/{id}")]
	public async Task<ActionResult> ChangeMontant(int id, [FromQuery] decimal montant)
	{
		await _demandeRepository.ChangeMTAsync(id, montant);
		return NoContent();
	}

	[HttpPatch("ChangeMedicaments/{id}")]
	public async Task<ActionResult> ChangeMedicaments(int id, [FromBody] List<MedicamentDTO> medicaments)
	{
		await _demandeRepository.ChangeMedicAsync(id, medicaments);
		return NoContent();
	}
}
