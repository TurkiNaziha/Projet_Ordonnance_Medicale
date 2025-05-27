using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Repository;
using WebApplication1.Models;
using WebApplication1.DTO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _repository;
		private readonly IMedecinRepository _medecinRepository;

		private readonly DsContext _context;

		public PatientController(IPatientRepository repository, DsContext dsContext)
        {
            _repository = repository;
            _medecinRepository = _medecinRepository;
            _context = dsContext;

		}

		// GET: api/patient

	
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			// Fetch patients without relying on the navigation property
			var patients = await _repository.GetAllAsync();

			// Map patients to PatientDTOs, explicitly fetching Medecin details
			//var patientDTOs = new List<PatientDTO>();

			//foreach (var patient in patients)
			//{
			//	Console.WriteLine($"Patient: {patient.Id}, MedecinID: {patient.MedecinID}");
			//	// Fetch Medecin details based on MedecinID
			//	Medecin medecin = await _medecinRepository.GetByIdAsync(patient.MedecinID);

			//	patientDTOs.Add(new PatientDTO
			//	{
			//		Id = patient.Id,
			//		Nom = patient.Nom,
			//		Prenom = patient.Prenom,
			//		Mail = patient.Mail,
			//		Telephone = patient.Telephone,
			//		Maladie = patient.Maladie,
			//		MedecinID = patient.MedecinID,
			//		MedecinName = medecin != null ? $"{medecin.Nom} {medecin.Prenom}" : "Unknown"
			//	});
			//}

			var patientsWithMedecins = await _context.Patients
	.Include(p => p.Medecin) // Include navigation property
	.ToListAsync();

			var patientDTOs = patientsWithMedecins.Select(patient => new PatientDTO
			{
				Id = patient.Id,
				Nom = patient.Nom,
				Prenom = patient.Prenom,
				Mail = patient.Mail,
				Telephone = patient.Telephone,
				Maladie = patient.Maladie,
				MedecinID = patient.MedecinID , // Default value if null
				MedecinName = patient.Medecin != null ? $"{patient.Medecin.Nom} {patient.Medecin.Prenom}" : "Unknown"
			}).ToList();

			return Ok(patientDTOs);


			
		}


		// GET: api/patient/5
		[HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) return NotFound();

            return Ok(patient);
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(PatientDTO dto)
        {
            var patient = new Patient
            {
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Mail = dto.Mail,
                Telephone =dto.Telephone,
                Maladie = dto.Maladie,
                MedecinID = dto.MedecinID 
            };

            await _repository.AddAsync(patient);
            return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PatientDTO dto)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) return NotFound();

            patient.Nom = dto.Nom;
            patient.Prenom = dto.Prenom;
            patient.Mail = dto.Mail;
            patient.Telephone = dto.Telephone;
            patient.Maladie = dto.Maladie;
            patient.MedecinID = dto.MedecinID; 

            await _repository.UpdateAsync(patient);
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}