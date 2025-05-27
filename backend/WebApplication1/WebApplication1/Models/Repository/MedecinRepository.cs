using Microsoft.EntityFrameworkCore;



namespace WebApplication1.Models.Repository
{
    public class MedecinRepository : IMedecinRepository
    {
        private readonly DsContext _context;

        public MedecinRepository(DsContext context)
        {
            _context = context;
        }

        public async Task<List<Medecin>> GetAllAsync()
        {
            try
            {
                // Fetch all Medecins
                return await _context.Medecins.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching Medecins: {ex.Message}");
                throw;
            }
        }

        public async Task<Medecin> GetByIdAsync(int id)
        {
            try
            {
                // Fetch Medecin by Id
                return await _context.Medecins.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching Medecin by Id: {ex.Message}");
                throw;
            }
        }

        public async Task AddAsync(Medecin medecin)
        {
            try
            {
                // Add a new Medecin
                await _context.Medecins.AddAsync(medecin);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding Medecin: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAsync(Medecin medecin)
        {
            try
            {
                // Update an existing Medecin
                _context.Medecins.Update(medecin);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating Medecin: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                // Delete a Medecin by Id
                var medecin = await GetByIdAsync(id);
                if (medecin != null)
                {
                    _context.Medecins.Remove(medecin);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting Medecin: {ex.Message}");
                throw;
            }
        }
    }
}
