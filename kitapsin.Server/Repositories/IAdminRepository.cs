using kitapsin.Server.Models;

namespace kitapsin.Server.Repositories
{
    public interface IAdminRepository
    {
        Task<IEnumerable<Admin>> GetAllAsync();
        Task<Admin?> GetByIdAsync(int id);
        Task<Admin?> GetByUsernameAsync(string username);
        Task AddAsync(Admin admin);
        Task DeleteAsync(Admin admin);
        Task SaveChangesAsync();
    }
}
