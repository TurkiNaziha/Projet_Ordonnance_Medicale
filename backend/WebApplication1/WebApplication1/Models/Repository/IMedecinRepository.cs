namespace WebApplication1.Models.Repository
{
    public interface IMedecinRepository
    {
        Task<List<Medecin>> GetAllAsync();
        Task<Medecin> GetByIdAsync(int id);
        Task AddAsync(Medecin medecin);
        Task UpdateAsync(Medecin medecin);
        Task DeleteAsync(int id);
    }
}
