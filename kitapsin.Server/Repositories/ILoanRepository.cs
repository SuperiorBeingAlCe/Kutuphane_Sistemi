using kitapsin.Server.Models;

namespace kitapsin.Server.Repositories
{
    public interface ILoanRepository
    {
        Task AddAsync(Loan loan);
        Task DeleteAsync(Loan loan);

        Task<Loan?> GetByIdAsync(int id);
        Task<IEnumerable<Loan>> GetAllAsync();
        Task SaveChangesAsync();
    }
}
