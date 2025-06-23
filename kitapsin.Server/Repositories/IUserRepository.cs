using kitapsin.Server.Models;

namespace kitapsin.Server.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByCardNumberAsync(string cardNumber);
        Task<IEnumerable<User>> SearchByNameAsync(string name);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task SaveChangesAsync();
        Task<string?> GetLastCardNumberAsync();

        Task<IEnumerable<Loan>> GetLoansAsync(int userId);
        Task<IEnumerable<Penalty>> GetPenaltiesAsync(int userId);
        Task<IEnumerable<Book>> GetBorrowedBooksAsync(int userId);
    }
}
