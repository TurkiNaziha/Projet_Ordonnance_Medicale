using System;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models.Repository
{
	public class MedicamentRepository : IMedicamentRepository
	{
		private readonly DsContext _context;

		public MedicamentRepository(DsContext context)
		{
			_context = context;
		}

		public async Task<List<Medicament>> GetAllAsync()
		{
			try
			{
				// Attempt to retrieve the list of Medicaments
				List<Medicament> medicaments = await _context.Medicaments.ToListAsync();
				return medicaments;
			}
			catch (Exception ex)
			{
				// Log the error message to the console or a logging service
				Console.WriteLine($"An error occurred while fetching Medicaments: {ex.Message}");
				Console.WriteLine($"Stack Trace: {ex.StackTrace}");

				// Optionally, rethrow the exception if you want it to propagate
				throw;
			}
		}


		public async Task<Medicament> GetByIdAsync(int id)
		{
			return await _context.Medicaments.FindAsync(id);
		}

		public async Task AddAsync(Medicament medicament)
		{
			await _context.Medicaments.AddAsync(medicament);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Medicament medicament)
		{
			_context.Medicaments.Update(medicament);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var medicament = await GetByIdAsync(id);
			if (medicament != null)
			{
				_context.Medicaments.Remove(medicament);
				await _context.SaveChangesAsync();
			}
		}
	}
}
