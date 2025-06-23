using kitapsin.Server.Dto;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Kullanıcı işlemleri için servis arayüzü.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Tüm kullanıcıları getirir.
        /// </summary>
        /// <returns>Kullanıcı yanıtlarının listesi.</returns>
        Task<IEnumerable<DtoUserResponse>> GetAllAsync();

        /// <summary>
        /// Belirli bir kullanıcıyı kimliğine göre getirir.
        /// </summary>
        /// <param name="id">Kullanıcı kimliği.</param>
        /// <returns>Kullanıcı yanıtı.</returns>
        Task<DtoUserResponse> GetByIdAsync(int id);

        /// <summary>
        /// Kart numarasına göre kullanıcıyı getirir.
        /// </summary>
        /// <param name="cardNumber">Kullanıcı kart numarası.</param>
        /// <returns>Kullanıcı yanıtı.</returns>
        Task<DtoUserResponse> GetByCardNumberAsync(string cardNumber);

        /// <summary>
        /// İsme göre kullanıcıları arar.
        /// </summary>
        /// <param name="name">Kullanıcı adı.</param>
        /// <returns>Kullanıcı yanıtlarının listesi.</returns>
        Task<IEnumerable<DtoUserResponse>> SearchByNameAsync(string name);

        /// <summary>
        /// Yeni kullanıcı oluşturur.
        /// </summary>
        /// <param name="dto">Kullanıcı oluşturma DTO'su.</param>
        /// <returns>Oluşturulan kullanıcı yanıtı.</returns>
        Task<DtoUserResponse> CreateAsync(DtoUserCreate dto);

        /// <summary>
        /// Kullanıcıyı günceller.
        /// </summary>
        /// <param name="id">Kullanıcı kimliği.</param>
        /// <param name="dto">Kullanıcı güncelleme DTO'su.</param>
        /// <returns>Güncelleme başarılıysa true, aksi halde false.</returns>
        Task<bool> UpdateAsync(int id, DtoUserUpdate dto);

        /// <summary>
        /// Kullanıcıyı siler.
        /// </summary>
        /// <param name="id">Kullanıcı kimliği.</param>
        /// <returns>Silme başarılıysa true, aksi halde false.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Kullanıcının ödünç aldığı kitapları getirir.
        /// </summary>
        /// <param name="userId">Kullanıcı kimliği.</param>
        /// <returns>Ödünç alma yanıtlarının listesi.</returns>
        Task<IEnumerable<DtoLoanResponse>> GetLoansAsync(int userId);

        /// <summary>
        /// Kullanıcının cezalarını getirir.
        /// </summary>
        /// <param name="userId">Kullanıcı kimliği.</param>
        /// <returns>Ceza yanıtlarının listesi.</returns>
        Task<IEnumerable<DtoPenaltyResponse>> GetPenaltiesAsync(int userId);

        /// <summary>
        /// Kullanıcının ödünç aldığı kitapların listesini getirir.
        /// </summary>
        /// <param name="userId">Kullanıcı kimliği.</param>
        /// <returns>Kitap yanıtlarının listesi.</returns>
        Task<IEnumerable<DtoBookResponse>> GetBorrowedBooksAsync(int userId);
    }
}
