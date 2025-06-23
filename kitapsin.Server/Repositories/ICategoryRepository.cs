
using kitapsin.Server.Models;

namespace kitapsin.Server.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task<IEnumerable<Category>> SearchByNameAsync(string name);
        Task DeleteAsync(Category category);
        Task SaveChangesAsync();
        Task<IEnumerable<Book>> GetBooksByCategoryIdAsync(int categoryId);
    }
}
