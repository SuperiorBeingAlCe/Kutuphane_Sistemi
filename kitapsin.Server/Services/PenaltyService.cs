using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Models;
using kitapsin.Server.Repositories;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Ceza işlemleri için servis.
    /// </summary>
    public class PenaltyService : IPenaltyService
    {
        private readonly IPenaltyRepository _repo;

        /// <summary>
        /// PenaltyService sınıfının kurucusu.
        /// </summary>
        /// <param name="repo">Ceza repository bağımlılığı.</param>
        public PenaltyService(IPenaltyRepository repo) => _repo = repo;

        /// <inheritdoc/>
        public async Task<DtoPenaltyResponse> AddAsync(DtoPenaltyCreate dto)
        {
            if (dto.UserId <= 0 || dto.Amount <= 0)
                throw new MyCustomException("Geçersiz kullanıcı veya ceza miktarı.");

            var penalty = new Penalty
            {
                UserId = dto.UserId,
                Reason = dto.Reason,
                Amount = dto.Amount,
                IssuedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(penalty);
            await _repo.SaveChangesAsync();

            return new DtoPenaltyResponse
            {
                Id = penalty.Id,
                Reason = penalty.Reason,
                Amount = penalty.Amount,
                IssuedAt = penalty.IssuedAt,
                IsPaid = false
            };
        }

        /// <inheritdoc/>
        public async Task<bool> PayAndRemoveAsync(int id)
        {
            var penalty = await _repo.GetByIdAsync(id);
            if (penalty == null)
                throw new MyCustomException($"Silinecek ceza kaydı bulunamadı. Id={id}");

            penalty.IsPaid = true;
            await _repo.DeleteAsync(penalty);
            await _repo.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<DtoPenaltyResponse?> GetByIdAsync(int id)
        {
            var penalty = await _repo.GetByIdAsync(id);
            if (penalty == null)
                return null;

            return new DtoPenaltyResponse
            {
                Id = penalty.Id,
                UserId = penalty.UserId,
                Reason = penalty.Reason,
                Amount = penalty.Amount,
                IssuedAt = penalty.IssuedAt,
                IsPaid = penalty.IsPaid
            };
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DtoPenaltyResponse>> GetAllAsync()
        {
            var penalties = await _repo.GetAllAsync();
            return penalties.Select(p => new DtoPenaltyResponse
            {
                Id = p.Id,
                UserId = p.UserId,
                Reason = p.Reason,
                Amount = p.Amount,
                IssuedAt = p.IssuedAt,
                IsPaid = p.IsPaid
            });
        }
    }
}
