using kitapsin.Server.Models;

namespace kitapsin.Server.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author?> GetByIdAsync(int id);
        Task<IEnumerable<Author>> SearchByTitleAsync(string name);
        Task AddAsync(Author author);
        Task UpdateAsync(Author author);
        Task DeleteAsync(Author author);
        Task SaveChangesAsync();
        Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId);
    }
}
