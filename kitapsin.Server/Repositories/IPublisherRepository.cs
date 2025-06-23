using kitapsin.Server.Models;

namespace kitapsin.Server.Repositories
{
    public interface IPublisherRepository
    {
        Task<IEnumerable<Publisher>> GetAllAsync();
        Task<Publisher?> GetByIdAsync(int id);
        Task AddAsync(Publisher publisher);
        Task UpdateAsync(Publisher publisher);
        Task<IEnumerable<Publisher>> SearchByNameAsync(string name);
        Task DeleteAsync(Publisher publisher);
        Task SaveChangesAsync();
        Task<IEnumerable<Book>> GetBooksByPublisherIdAsync(int publisherId);
    }
}
