using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Models;
using kitapsin.Server.Repositories;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Ödünç işlemleri için servis.
    /// </summary>
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _repo;

        /// <summary>
        /// LoanService sınıfının yeni bir örneğini başlatır.
        /// </summary>
        /// <param name="repo">Ödünç işlemleri için repository.</param>
        public LoanService(ILoanRepository repo) => _repo = repo;

        /// <inheritdoc/>
        public async Task<DtoLoanResponse> AddAsync(DtoLoanCreate dto)
        {
            if (dto.BookId <= 0 || dto.UserId <= 0)
                throw new MyCustomException("Geçersiz kullanıcı veya kitap bilgisi.");

            var loan = new Loan
            {
                UserId = dto.UserId,
                BookId = dto.BookId,
                LoanDate = DateTime.UtcNow,
                DueDate = dto.DueDate
            };

            await _repo.AddAsync(loan);
            await _repo.SaveChangesAsync();

            return new DtoLoanResponse
            {
                Id = loan.Id,
                BookId = loan.BookId,
                BookTitle = dto.BookTitle,
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate
            };
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            var loan = await _repo.GetByIdAsync(id);
            if (loan == null)
                throw new MyCustomException($"Silinecek ödünç kaydı bulunamadı. Id={id}");

            await _repo.DeleteAsync(loan);
            await _repo.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<DtoLoanResponse?> GetByIdAsync(int id)
        {
            var loan = await _repo.GetByIdAsync(id);
            if (loan == null)
                return null;

            return new DtoLoanResponse
            {
                Id = loan.Id,
                BookId = loan.BookId,
                BookTitle = loan.Book?.Title ?? string.Empty,
                UserId = loan.UserId,
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate,
            };
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DtoLoanResponse>> GetAllAsync()
        {
            var loans = await _repo.GetAllAsync();
            return loans.Select(loan => new DtoLoanResponse
            {
                Id = loan.Id,
                BookId = loan.BookId,
                BookTitle = loan.Book?.Title ?? string.Empty,
                UserId = loan.UserId,
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate
            });
        }
    }
}
