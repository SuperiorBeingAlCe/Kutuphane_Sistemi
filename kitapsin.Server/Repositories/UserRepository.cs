using kitapsin.Server.Models;
using Microsoft.EntityFrameworkCore;


namespace kitapsin.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
            => await _context.Users.ToListAsync();

        public async Task<User?> GetByIdAsync(int id)
            => await _context.Users.FindAsync(id);

        public async Task<User?> GetByCardNumberAsync(string cardNumber)
            => await _context.Users.FirstOrDefaultAsync(u => u.CardNumber == cardNumber);

        public async Task<IEnumerable<User>> SearchByNameAsync(string name)
            => await _context.Users
                .Where(u => u.FullName.Contains(name))
                .ToListAsync();

        public async Task AddAsync(User user)
            => await _context.Users.AddAsync(user);

        public Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public async Task<IEnumerable<Loan>> GetLoansAsync(int userId)
            => await _context.Loans
                .Where(l => l.UserId == userId)
                .ToListAsync();

        public async Task<IEnumerable<Penalty>> GetPenaltiesAsync(int userId)
            => await _context.Penalties
                .Where(p => p.UserId == userId)
                .ToListAsync();

        public async Task<IEnumerable<Book>> GetBorrowedBooksAsync(int userId)
        {
            return await _context.Loans
                .Where(l => l.UserId == userId && l.ReturnDate == null) 
                .Include(l => l.Book)
                .Select(l => l.Book)
                .ToListAsync();
        }
        public async Task<string?> GetLastCardNumberAsync()
        {
            return await _context.Users
                .OrderByDescending(u => u.CardNumber)
                .Select(u => u.CardNumber)
                .FirstOrDefaultAsync();
        }
    }
}

