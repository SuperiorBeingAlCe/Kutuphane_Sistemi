using kitapsin.Server.Dto;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Kitap işlemleri için servis arayüzü.
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Tüm kitapları getirir.
        /// </summary>
        /// <returns>Tüm kitapların listesi.</returns>
        Task<IEnumerable<DtoBookResponse>> GetAllAsync();

        /// <summary>
        /// Belirtilen ID'ye sahip kitabı getirir.
        /// </summary>
        /// <param name="id">Kitap ID'si.</param>
        /// <returns>Kitap bilgisi.</returns>
        Task<DtoBookResponse> GetByIdAsync(int id);

        /// <summary>
        /// Başlığa göre kitap arar.
        /// </summary>
        /// <param name="title">Kitap başlığı.</param>
        /// <returns>Arama sonucunda bulunan kitaplar.</returns>
        Task<IEnumerable<DtoBookResponse>> SearchByTitleAsync(string title);

        /// <summary>
        /// Yeni kitap oluşturur.
        /// </summary>
        /// <param name="dto">Kitap oluşturma DTO'su.</param>
        /// <returns>Oluşturulan kitabın bilgisi.</returns>
        Task<DtoBookResponse> CreateAsync(DtoBookCreate dto);

        /// <summary>
        /// Kitap bilgisini günceller.
        /// </summary>
        /// <param name="id">Kitap ID'si.</param>
        /// <param name="dto">Güncellenecek bilgiler.</param>
        /// <returns>Güncelleme başarılıysa true, değilse false.</returns>
        Task<bool> UpdateAsync(int id, DtoBookUpdate dto);

        /// <summary>
        /// Kitabı siler.
        /// </summary>
        /// <param name="id">Kitap ID'si.</param>
        /// <returns>Silme başarılıysa true, değilse false.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Kitabın yazarını değiştirir.
        /// </summary>
        /// <param name="bookId">Kitap ID'si.</param>
        /// <param name="newAuthorId">Yeni yazar ID'si.</param>
        Task ChangeBookAuthorAsync(int bookId, int newAuthorId);
    }
}
