using kitapsin.Server.Models;

namespace kitapsin.Server.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task<IEnumerable<Book>> SearchByTitleAsync(string title);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Book book);
        Task SaveChangesAsync();

        Task ChangeAuthorAsync(int bookId, int newAuthorId);
    }
}
