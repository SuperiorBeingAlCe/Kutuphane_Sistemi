using kitapsin.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace kitapsin.Server.Repositories
{
    public class ShelfLayoutPreferenceRepository : IShelfLayoutPreferenceRepository
    {
        private readonly ApplicationDbContext _context;

        public ShelfLayoutPreferenceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShelfLayoutPreference?> GetByAdminIdAsync(int adminId)
        {
            return await _context.ShelfLayoutPreferences
                .FirstOrDefaultAsync(x => x.AdminId == adminId);
        }

        public async Task SetPreferenceAsync(int adminId, bool isBlockLayout)
        {
            var existing = await GetByAdminIdAsync(adminId);
            if (existing == null)
            {
                var pref = new ShelfLayoutPreference
                {
                    AdminId = adminId,
                    IsBlockLayout = isBlockLayout
                };
                await _context.ShelfLayoutPreferences.AddAsync(pref);
            }
            else
            {
                existing.IsBlockLayout = isBlockLayout;
                _context.ShelfLayoutPreferences.Update(existing);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
