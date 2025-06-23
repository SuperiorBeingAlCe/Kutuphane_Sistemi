using kitapsin.Server.Dto;
using kitapsin.Server.Models;
using kitapsin.Server.Repositories;

using kitapsin.Server.Exceptions;

namespace kitapsin.Server.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repo;

        public AdminService(IAdminRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<DtoAdminResponse>> GetAllAsync()
        {
            var admins = await _repo.GetAllAsync();
            return admins.Select(a => new DtoAdminResponse
            {
                Id = a.Id,
                Username = a.Username,
                Email = a.Email
            });
        }

        public async Task<DtoAdminResponse> GetByIdAsync(int id)
        {
            var admin = await _repo.GetByIdAsync(id);
            if (admin is null)
            {
                throw new MyCustomException("Admin bulunamadı");
            }

            return new DtoAdminResponse
            {
                Id = admin.Id,
                Username = admin.Username,
                Email = admin.Email,
               
            };
        }

        public async Task<DtoAdminResponse> GetByUsernameAsync(string username)
        {
            var admin = await _repo.GetByUsernameAsync(username);
            if (admin is null)
                throw new MyCustomException($"Admin bulunamadı. Kullanıcı adı='{username}'");

            return new DtoAdminResponse
            {
                Id = admin.Id,
                Username = admin.Username,
                Email = admin.Email
            };
        }

        public async Task<DtoAdminResponse> CreateAsync(DtoAdminCreate dto)
        {
            // Aynı kullanıcı adı veya e-posta kontrolü
            if (await _repo.GetByUsernameAsync(dto.Username) is not null)
                throw new MyCustomException($"Bu kullanıcı adı zaten kullanılıyor: '{dto.Username}'");

            if ((await _repo.GetAllAsync()).Any(a => a.Email == dto.Email))
                throw new MyCustomException($"Bu email adresi zaten kayıtlı: '{dto.Email}'");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var admin = new Admin
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = hashedPassword
            };

            await _repo.AddAsync(admin);
            await _repo.SaveChangesAsync();

            return new DtoAdminResponse
            {
                Id = admin.Id,
                Username = admin.Username,
                Email = admin.Email
            };
        }

        public async Task<bool> ValidateLoginAsync(string username, string password)
        {
            var admin = await _repo.GetByUsernameAsync(username);
            if (admin is null)
                throw new MyCustomException("Kullanıcı adı veya şifre hatalı.");

            if (!BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
                throw new MyCustomException("Kullanıcı adı veya şifre hatalı.");

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var admin = await _repo.GetByIdAsync(id);
            if (admin is null)
                throw new MyCustomException($"Silinecek admin bulunamadı. Id={id}");

            await _repo.DeleteAsync(admin);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
