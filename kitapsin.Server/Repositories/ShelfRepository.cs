using kitapsin.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace kitapsin.Server.Repositories
{
    public class ShelfRepository : IShelfRepository

    {
        private readonly ApplicationDbContext _context;

        public ShelfRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shelf>> GetAllAsync()
        {
            return await _context.Shelves.ToListAsync();
        }

        public async Task<Shelf?> GetByIdAsync(int id)
        {
            return await _context.Shelves.FindAsync(id);
        }

        public async Task AddAsync(Shelf shelf)
        {
            await _context.Shelves.AddAsync(shelf);
        }

      
        

        public Task DeleteAsync(Shelf shelf)
        {
            _context.Shelves.Remove(shelf);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetAllBooksInShelfIdAsync(int shelfId)
        {
            var shelf = await _context.Shelves
                .Include(s => s.Books)
                .FirstOrDefaultAsync(s => s.Id == shelfId);

            return shelf?.Books ?? Enumerable.Empty<Book>();
        }

        public async Task AddBookIntoShelf(int shelfId, int bookId)
        {
            var shelf = await _context.Shelves
                .Include(s => s.Books)
                .FirstOrDefaultAsync(s => s.Id == shelfId);

            var book = await _context.Books.FindAsync(bookId);

            shelf?.Books.Add(book!); 
        }

        public async Task RemoveBookFromShelfAsync(int shelfId, int bookId)
        {
            var shelf = await _context.Shelves
                .Include(s => s.Books)
                .FirstOrDefaultAsync(s => s.Id == shelfId);

            var book = await _context.Books.FindAsync(bookId);

            shelf?.Books.Remove(book!);
        }
    }
}
    
    
