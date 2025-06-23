
using kitapsin.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace kitapsin.Server.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly ApplicationDbContext _context;

        public PublisherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync()
        {
            return await _context.Publishers.ToListAsync();
        }

        public async Task<Publisher?> GetByIdAsync(int id)
        {
            return await _context.Publishers.FindAsync(id);
        }

        public async Task AddAsync(Publisher publisher)
        {
            await _context.Publishers.AddAsync(publisher);
        }

        public  Task UpdateAsync(Publisher publisher)
        {
            _context.Publishers.Update(publisher);
            return Task.CompletedTask;
        }

        public  Task DeleteAsync(Publisher publisher)
        {
            _context.Publishers.Remove(publisher);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByPublisherIdAsync(int publisherId)
        {
            var publisher = await _context.Publishers
                                          .Include(p => p.Books)
                                          .FirstOrDefaultAsync(p => p.Id == publisherId);

            return publisher?.Books ?? Enumerable.Empty<Book>();
        }
        public async Task<IEnumerable<Publisher>> SearchByNameAsync(string name)
        {
            return await _context.Publishers
                .Include(p => p.Books)
                .Where(p => EF.Functions.Like(p.Name, $"%{name}%"))
                .ToListAsync();
        }

        

    }
}
