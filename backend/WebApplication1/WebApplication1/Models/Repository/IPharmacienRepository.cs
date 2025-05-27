namespace WebApplication1.Models.Repository
{
    public interface IPharmacienRepository
    {
        Task<List<Pharmacien>> GetAllAsync();
        Task<Pharmacien> GetByIdAsync(int id);
        Task AddAsync(Pharmacien pharmacien);
        Task UpdateAsync(Pharmacien pharmacien);
        Task DeleteAsync(int id);
    }
}
