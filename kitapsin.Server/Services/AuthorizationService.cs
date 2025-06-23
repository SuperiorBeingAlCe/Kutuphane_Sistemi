using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Repositories;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Yetkilendirme işlemlerini yöneten servis.
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAdminRepository _adminRepo;
        private readonly JwtTokenService _jwtService;

        /// <summary>
        /// AuthorizationService sınıfının yeni bir örneğini başlatır.
        /// </summary>
        /// <param name="adminRepo">Yönetici veritabanı deposu.</param>
        /// <param name="jwtService">JWT token servisi.</param>
        public AuthorizationService(IAdminRepository adminRepo, JwtTokenService jwtService)
        {
            _adminRepo = adminRepo;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Yönetici girişi yapar ve JWT token döner.
        /// </summary>
        /// <param name="dto">Giriş bilgileri DTO'su.</param>
        /// <returns>JWT token stringi.</returns>
        /// <exception cref="MyCustomException">Kullanıcı adı veya şifre hatalıysa fırlatılır.</exception>
        public async Task<string> LoginAsync(DtoAdminLogin dto)
        {
            var admin = await _adminRepo.GetByUsernameAsync(dto.Username);
            if (admin is null)
                throw new MyCustomException("Kullanıcı adı veya şifre hatalı.");

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, admin.PasswordHash);
            if (!isPasswordValid)
                throw new MyCustomException("Kullanıcı adı veya şifre hatalı.");

            return _jwtService.GenerateToken(admin);
        }
    }
}
