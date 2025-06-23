
using kitapsin.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace kitapsin.Server.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;

        

        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors.Include(a => a.Books).ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _context.Authors.Include(a => a.Books)
                                         .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Author>> SearchByTitleAsync(string name)
        {
            return await _context.Authors
                                 .Where(a => a.Name.Contains(name))
                                 .Include(a => a.Books)
                                 .ToListAsync();
        }

        public async Task AddAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
        }

        public Task UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Author author)
        {
            _context.Authors.Remove(author);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId)
        {
            return await _context.Books
                         .Include(b => b.Author)
                         .Where(b => b.AuthorId == authorId)
                         .ToListAsync();
        }


    }
}
