using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Repository;

namespace WebApplication1.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PharmacienController : ControllerBase
	{
		private readonly IPharmacienRepository _repository;

		public PharmacienController(IPharmacienRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			List<Pharmacien> pharmaciens = await _repository.GetAllAsync();
			return Ok(pharmaciens);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var pharmacien = await _repository.GetByIdAsync(id);
			if (pharmacien == null) return NotFound();

			return Ok(pharmacien);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Pharmacien dto)
		{
			var pharmacien = new Pharmacien
			{
				Nom = dto.Nom,
				Prenom = dto.Prenom,
				Email = dto.Email
			};

			await _repository.AddAsync(pharmacien);
			return CreatedAtAction(nameof(GetById), new { id = pharmacien.Id }, pharmacien);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, Pharmacien dto)
		{
			var pharmacien = await _repository.GetByIdAsync(id);
			if (pharmacien == null) return NotFound();

			pharmacien.Nom = dto.Nom;
			pharmacien.Prenom = dto.Prenom;
			pharmacien.Email = dto.Email;

			await _repository.UpdateAsync(pharmacien);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var pharmacien = await _repository.GetByIdAsync(id);
			if (pharmacien == null) return NotFound();

			await _repository.DeleteAsync(id);
			return NoContent();
		}
	}
}
