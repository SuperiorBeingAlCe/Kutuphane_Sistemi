using kitapsin.Server.Models;
using kitapsin.Server.Repositories;

namespace kitapsin.Server.Services
{
    public class ShelfLayoutPreferenceService : IShelfLayoutPreferenceService
    {
        private readonly IShelfLayoutPreferenceRepository _repo;

        public ShelfLayoutPreferenceService(IShelfLayoutPreferenceRepository repo)
        {
            _repo = repo;
        }

        public async Task<ShelfLayoutPreference?> GetPreferenceAsync(int userId)
        {
            return await _repo.GetByAdminIdAsync(userId);
        }

        public async Task SetPreferenceAsync(int userId, bool IsBlockLayout)
        {
            await _repo.SetPreferenceAsync(userId, IsBlockLayout);
            await _repo.SaveChangesAsync();
        }

        public async Task DeletePreferenceAsync(int userId)
        {
            // Silme işlemi için repository'de bir metot yok, bu yüzden boş bırakıldı.
            await Task.CompletedTask;
        }
    }
}
