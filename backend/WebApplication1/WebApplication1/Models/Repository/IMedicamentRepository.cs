namespace WebApplication1.Models.Repository
{
	public interface IMedicamentRepository
	{
		Task<List<Medicament>> GetAllAsync();
		Task<Medicament> GetByIdAsync(int id);
		Task AddAsync(Medicament medicament);
		Task UpdateAsync(Medicament medicament);
		Task DeleteAsync(int id);
	}
}
