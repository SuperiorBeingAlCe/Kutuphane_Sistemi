using kitapsin.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace kitapsin.Server.Repositories
{
    public class PenaltyRepository : IPenaltyRepository
    {


    private readonly ApplicationDbContext _context;
        public PenaltyRepository(ApplicationDbContext context) => _context = context;

        public async Task AddAsync(Penalty penalty)
        {
            await _context.Penalties.AddAsync(penalty);
        }

        public async Task<Penalty?> GetByIdAsync(int id)
        {
            return await _context.Penalties.FindAsync(id);
        }

        public Task DeleteAsync(Penalty penalty)
        {
            _context.Penalties.Remove(penalty);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Penalty>> GetAllAsync()
        {
            return await _context.Penalties.ToListAsync();
        }

    }
}
