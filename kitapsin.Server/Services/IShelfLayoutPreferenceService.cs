using kitapsin.Server.Models;

namespace kitapsin.Server.Services
{
    public interface IShelfLayoutPreferenceService
    {
        Task<ShelfLayoutPreference?> GetPreferenceAsync(int userId);
        Task SetPreferenceAsync(int userId, bool IsBlockLayout);
        Task DeletePreferenceAsync(int userId);
    }
}
