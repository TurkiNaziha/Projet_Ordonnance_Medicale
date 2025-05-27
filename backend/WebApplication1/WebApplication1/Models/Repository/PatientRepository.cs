using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
namespace WebApplication1.Models.Repository
{
	public class PatientRepository : IPatientRepository
	{
		private readonly DsContext _context;

		public PatientRepository(DsContext context)
		{
			_context = context;
		}

		public async Task<List<Patient>> GetAllAsync()
		{
			return await _context.Patients.ToListAsync();
		}

		public async Task<Patient> GetByIdAsync(int id)
		{
			return await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task AddAsync(Patient patient)
		{
			await _context.Patients.AddAsync(patient);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Patient patient)
		{
			_context.Patients.Update(patient);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var patient = await _context.Patients.FindAsync(id);
			if (patient != null)
			{
				_context.Patients.Remove(patient);
				await _context.SaveChangesAsync();
			}
		}
	}
}
