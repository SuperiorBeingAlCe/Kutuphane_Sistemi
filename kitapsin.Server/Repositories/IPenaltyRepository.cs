using kitapsin.Server.Models;

namespace kitapsin.Server.Repositories
{
    public interface IPenaltyRepository
    {
        Task AddAsync(Penalty penalty);
        Task<Penalty?> GetByIdAsync(int id);
        Task DeleteAsync(Penalty penalty);
        Task<IEnumerable<Penalty>> GetAllAsync();

        Task SaveChangesAsync();
    }
}
