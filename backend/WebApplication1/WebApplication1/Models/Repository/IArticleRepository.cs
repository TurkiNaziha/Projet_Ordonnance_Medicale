namespace WebApplication1.Models.Repository
{
	public interface IArticleRepository
	{
		Task<List<Article>> GetAllAsync();
		Task<Article> GetByIdAsync(int id);
		Task AddAsync(Article article);
		Task UpdateAsync(Article article);
		Task DeleteAsync(int id);
	}
}
