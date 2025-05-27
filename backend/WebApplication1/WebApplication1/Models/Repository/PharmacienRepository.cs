using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models.Repository
{
    public class PharmacienRepository : IPharmacienRepository
    {
        private readonly DsContext _context;

        public PharmacienRepository(DsContext context)
        {
            _context = context;
        }

        public async Task<List<Pharmacien>> GetAllAsync()
        {
            try
            {
                // Récupérer tous les pharmaciens
                return await _context.Pharmaciens.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la récupération des pharmaciens: {ex.Message}");
                throw;
            }
        }

        public async Task<Pharmacien> GetByIdAsync(int id)
        {
            try
            {
                // Récupérer un pharmacien par ID
                return await _context.Pharmaciens.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la récupération du pharmacien: {ex.Message}");
                throw;
            }
        }

        public async Task AddAsync(Pharmacien pharmacien)
        {
            try
            {
                // Ajouter un nouveau pharmacien
                await _context.Pharmaciens.AddAsync(pharmacien);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de l'ajout du pharmacien: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAsync(Pharmacien pharmacien)
        {
            try
            {
                // Mettre à jour un pharmacien existant
                _context.Pharmaciens.Update(pharmacien);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la mise à jour du pharmacien: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                // Supprimer un pharmacien
                var pharmacien = await GetByIdAsync(id);
                if (pharmacien != null)
                {
                    _context.Pharmaciens.Remove(pharmacien);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la suppression du pharmacien: {ex.Message}");
                throw;
            }
        }
    }
}
