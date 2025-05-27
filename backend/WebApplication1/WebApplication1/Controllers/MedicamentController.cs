using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Repository;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{


	[ApiController]
	[Route("api/[controller]")]
	public class MedicamentController : ControllerBase
	{
		private readonly IMedicamentRepository _repository;

		public MedicamentController(IMedicamentRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			List<Medicament> medicaments = await _repository.GetAllAsync();
			return Ok(medicaments);

		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var medicament = await _repository.GetByIdAsync(id);
			if (medicament == null) return NotFound();

			return Ok(medicament);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Medicament dto)
		{
			var medicament = new Medicament
			{
				Nom = dto.Nom,
				Label = dto.Label,
				Prix = dto.Prix,
				Image = dto.Image,
				Quantite = dto.Quantite
			};

			await _repository.AddAsync(medicament);
			return CreatedAtAction(nameof(GetById), new { id = medicament.Id }, medicament);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, Medicament dto)
		{
			var medicament = await _repository.GetByIdAsync(id);
			if (medicament == null) return NotFound();

			medicament.Nom = dto.Nom;
			medicament.Label = dto.Label;
			medicament.Prix = dto.Prix;
			medicament.Image = dto.Image;
			medicament.Quantite = dto.Quantite;

			await _repository.UpdateAsync(medicament);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _repository.DeleteAsync(id);
			return NoContent();
		}
	}
}
