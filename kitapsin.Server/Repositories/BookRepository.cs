using kitapsin.Server.Exceptions;
using kitapsin.Server.Models;
using Microsoft.EntityFrameworkCore;


namespace kitapsin.Server.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher) 
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> SearchByTitleAsync(string title)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Where(b => b.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync();
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
        }

        public  Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            return Task.CompletedTask;

        }

        public  Task DeleteAsync(Book book)
        {
            _context.Books.Remove(book);
            return Task.CompletedTask;

        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("HATA: " + ex.Message);

                if (ex.InnerException != null)
                {
                    Console.WriteLine("INNER: " + ex.InnerException.Message);

                    if (ex.InnerException.InnerException != null)
                        Console.WriteLine("INNER-2: " + ex.InnerException.InnerException.Message);
                }

                throw; 
            }
        }
        public async Task ChangeAuthorAsync(int bookId, int newAuthorId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                throw new MyCustomException("Kitap bulunamadı.");
            }
            book.AuthorId = newAuthorId;
            _context.Update(book);
            await _context.SaveChangesAsync();
        }
    }
}

