using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Repository;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedecinController : ControllerBase
    {
        private readonly IMedecinRepository _repository;

        public MedecinController(IMedecinRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Medecin> medecins = await _repository.GetAllAsync();
            return Ok(medecins);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var medecin = await _repository.GetByIdAsync(id);
            if (medecin == null) return NotFound();

            return Ok(medecin);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Medecin dto)
        {
            var medecin = new Medecin
            {
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Mail = dto.Mail,
                Telephone = dto.Telephone,
                Specialite = dto.Specialite
            };

            await _repository.AddAsync(medecin);
            return CreatedAtAction(nameof(GetById), new { id = medecin.Id }, medecin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Medecin dto)
        {
            var medecin = await _repository.GetByIdAsync(id);
            if (medecin == null) return NotFound();

            medecin.Nom = dto.Nom;
            medecin.Prenom = dto.Prenom;
            medecin.Mail = dto.Mail;
            medecin.Telephone = dto.Telephone;
            medecin.Specialite = dto.Specialite;

            await _repository.UpdateAsync(medecin);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var medecin = await _repository.GetByIdAsync(id);
            if (medecin == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
