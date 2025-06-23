using System.ComponentModel.DataAnnotations;
using AutoMapper;
using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Models;
using kitapsin.Server.Repositories;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Kullanıcı işlemleri için servis sınıfı.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        /// <summary>
        /// UserService sınıfının kurucusu.
        /// </summary>
        /// <param name="repo">Kullanıcı repository bağımlılığı.</param>
        /// <param name="mapper">AutoMapper bağımlılığı.</param>
        public UserService(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DtoUserResponse>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();
            if (!users.Any())
                throw new MyCustomException("Kayıtlı kullanıcı bulunamadı.");

            return _mapper.Map<IEnumerable<DtoUserResponse>>(users);
        }

        /// <inheritdoc/>
        public async Task<DtoUserResponse> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new MyCustomException("Geçersiz kullanıcı Id.");

            var user = await _repo.GetByIdAsync(id);
            if (user is null)
                throw new MyCustomException($"Kullanıcı bulunamadı. Id={id}");

            return _mapper.Map<DtoUserResponse>(user);
        }

        /// <inheritdoc/>
        public async Task<DtoUserResponse> GetByCardNumberAsync(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                throw new MyCustomException("Kart numarası boş olamaz.");

            var user = await _repo.GetByCardNumberAsync(cardNumber);
            if (user is null)
                throw new MyCustomException($"Kart numarası ile kullanıcı bulunamadı: {cardNumber}");

            return _mapper.Map<DtoUserResponse>(user);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DtoUserResponse>> SearchByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new MyCustomException("Arama için geçerli bir isim girilmeli.");

            var users = await _repo.SearchByNameAsync(name);
            if (!users.Any())
                throw new MyCustomException($"'{name}' içeren kullanıcı bulunamadı.");

            return _mapper.Map<IEnumerable<DtoUserResponse>>(users);
        }

        /// <inheritdoc/>
        public async Task<DtoUserResponse> CreateAsync(DtoUserCreate dto)
        {
            if (dto == null)
                throw new MyCustomException("Kullanıcı verisi boş olamaz.");

            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new MyCustomException("Ad Soyad boş olamaz.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new MyCustomException("Email boş olamaz.");

            if (!new EmailAddressAttribute().IsValid(dto.Email))
                throw new MyCustomException("Geçersiz email formatı.");

            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                throw new MyCustomException("Telefon numarası boş olamaz.");

            var existingByEmail = await _repo.GetAllAsync();
            if (existingByEmail.Any(u => u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
                throw new MyCustomException($"Bu email adresi zaten kayıtlı: {dto.Email}");

            // 🔁 Benzersiz kart numarası üret
            string newCard = await GenerateUniqueCardNumberAsync();

            var user = _mapper.Map<User>(dto);
            user.CardNumber = newCard;
            user.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();

            return _mapper.Map<DtoUserResponse>(user);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(int id, DtoUserUpdate dto)
        {
            if (id <= 0)
                throw new MyCustomException("Geçersiz kullanıcı Id.");

            var user = await _repo.GetByIdAsync(id);
            if (user is null)
                throw new MyCustomException($"Güncellenecek kullanıcı bulunamadı. Id={id}");

            user.FullName = dto.FullName ?? user.FullName;
            user.Email = dto.Email ?? user.Email;
            user.phoneNumber = dto.PhoneNumber ?? user.phoneNumber;

            await _repo.UpdateAsync(user);
            await _repo.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new MyCustomException("Geçersiz kullanıcı Id.");

            var user = await _repo.GetByIdAsync(id);
            if (user is null)
                throw new MyCustomException($"Silinecek kullanıcı bulunamadı. Id={id}");

            await _repo.DeleteAsync(user);
            await _repo.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DtoLoanResponse>> GetLoansAsync(int userId)
        {
            if (userId <= 0)
                throw new MyCustomException("Geçersiz kullanıcı Id.");

            var loans = await _repo.GetLoansAsync(userId);
            if (!loans.Any())
                throw new MyCustomException($"Kullanıcıya ait ödünç kaydı bulunamadı. UserId={userId}");

            return _mapper.Map<IEnumerable<DtoLoanResponse>>(loans);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DtoPenaltyResponse>> GetPenaltiesAsync(int userId)
        {
            if (userId <= 0)
                throw new MyCustomException("Geçersiz kullanıcı Id.");

            var penalties = await _repo.GetPenaltiesAsync(userId);
            if (!penalties.Any())
                throw new MyCustomException($"Kullanıcıya ait ceza kaydı bulunamadı. UserId={userId}");

            return _mapper.Map<IEnumerable<DtoPenaltyResponse>>(penalties);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DtoBookResponse>> GetBorrowedBooksAsync(int userId)
        {
            if (userId <= 0)
                throw new MyCustomException("Geçersiz kullanıcı Id.");

            var books = await _repo.GetBorrowedBooksAsync(userId);
            if (!books.Any())
                throw new MyCustomException($"Kullanıcıya ait ödünçteki kitap bulunamadı. UserId={userId}");

            return _mapper.Map<IEnumerable<DtoBookResponse>>(books);
        }

        /// <summary>
        /// Benzersiz bir kart numarası üretir.
        /// </summary>
        /// <returns>Benzersiz kart numarası.</returns>
        /// <exception cref="MyCustomException">Kart numarası oluşturulamazsa fırlatılır.</exception>
        private async Task<string> GenerateUniqueCardNumberAsync()
        {
            const int maxRetries = 5;
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                var lastCard = await _repo.GetLastCardNumberAsync();
                int nextNumber = 1;

                if (!string.IsNullOrEmpty(lastCard) && int.TryParse(lastCard, out int lastNumeric))
                    nextNumber = lastNumeric + 1;

                var newCard = nextNumber.ToString("D9");

                var existsCard = await _repo.GetByCardNumberAsync(newCard);
                if (existsCard == null)
                    return newCard;

                retryCount++;
            }

            throw new MyCustomException("Kart numarası oluşturulamadı. Lütfen tekrar deneyin.");
        }
    }
}
