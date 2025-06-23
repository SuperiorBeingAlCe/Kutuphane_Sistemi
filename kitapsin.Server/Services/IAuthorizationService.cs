using kitapsin.Server.Dto;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Yetkilendirme işlemleri için servis arayüzü.
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Yönetici girişi yapar ve JWT token döner.
        /// </summary>
        /// <param name="dto">Giriş bilgilerini içeren DTO.</param>
        /// <returns>JWT token string.</returns>
        Task<string> LoginAsync(DtoAdminLogin dto);
    }
}
