using kitapsin.Server.Dto;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Admin işlemleri için servis arayüzü.
    /// </summary>
    public interface IAdminService
    {
        /// <summary>
        /// Tüm adminleri getirir.
        /// </summary>
        /// <returns>Tüm adminlerin listesi.</returns>
        Task<IEnumerable<DtoAdminResponse>> GetAllAsync();

        /// <summary>
        /// Belirli bir admini ID ile getirir.
        /// </summary>
        /// <param name="id">Admin ID</param>
        /// <returns>Admin bilgisi.</returns>
        Task<DtoAdminResponse> GetByIdAsync(int id);

        /// <summary>
        /// Kullanıcı adına göre admin getirir.
        /// </summary>
        /// <param name="username">Kullanıcı adı</param>
        /// <returns>Admin bilgisi.</returns>
        Task<DtoAdminResponse> GetByUsernameAsync(string username);

        /// <summary>
        /// Yeni bir admin oluşturur.
        /// </summary>
        /// <param name="dto">Oluşturulacak admin bilgileri</param>
        /// <returns>Oluşturulan admin bilgisi.</returns>
        Task<DtoAdminResponse> CreateAsync(DtoAdminCreate dto);

        /// <summary>
        /// Admin giriş bilgilerini doğrular.
        /// </summary>
        /// <param name="username">Kullanıcı adı</param>
        /// <param name="password">Şifre</param>
        /// <returns>Doğrulama sonucu.</returns>
        Task<bool> ValidateLoginAsync(string username, string password);

        /// <summary>
        /// Admini siler.
        /// </summary>
        /// <param name="id">Admin ID</param>
        /// <returns>Silme işleminin sonucu.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
