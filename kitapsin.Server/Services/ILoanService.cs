using kitapsin.Server.Dto;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Ödünç işlemleri için servis arayüzü.
    /// </summary>
    public interface ILoanService
    {
        /// <summary>
        /// Yeni bir ödünç kaydı ekler.
        /// </summary>
        /// <param name="dto">Ödünç oluşturma DTO'su.</param>
        /// <returns>Eklenen ödünç kaydının yanıt DTO'su.</returns>
        Task<DtoLoanResponse> AddAsync(DtoLoanCreate dto);

        /// <summary>
        /// Belirtilen ID'ye sahip ödünç kaydını siler.
        /// </summary>
        /// <param name="id">Ödünç kaydının ID'si.</param>
        /// <returns>Silme işleminin başarılı olup olmadığını belirten değer.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Belirtilen ID'ye sahip ödünç kaydını getirir.
        /// </summary>
        /// <param name="id">Ödünç kaydının ID'si.</param>
        /// <returns>Ödünç kaydının yanıt DTO'su veya null.</returns>
        Task<DtoLoanResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Tüm ödünç kayıtlarını getirir.
        /// </summary>
        /// <returns>Ödünç kayıtlarının yanıt DTO'ları koleksiyonu.</returns>
        Task<IEnumerable<DtoLoanResponse>> GetAllAsync();
    }
}
