using kitapsin.Server.Models;

namespace kitapsin.Server.Repositories
{
    public interface IShelfLayoutPreferenceRepository
    {
        Task<ShelfLayoutPreference?> GetByAdminIdAsync(int adminId);
        Task SetPreferenceAsync(int adminId, bool isBlockLayout);
        Task SaveChangesAsync();
    }
}
