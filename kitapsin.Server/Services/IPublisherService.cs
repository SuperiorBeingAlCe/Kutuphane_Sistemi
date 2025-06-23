using kitapsin.Server.Dto;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Yayıncı ile ilgili işlemleri tanımlar.
    /// </summary>
    public interface IPublisherService
    {
        /// <summary>
        /// Tüm yayıncıları getirir.
        /// </summary>
        /// <returns>Tüm yayıncıların listesi.</returns>
        Task<IEnumerable<DtoPublisherResponse>> GetAllAsync();

        /// <summary>
        /// Belirtilen ID'ye sahip yayıncıyı getirir.
        /// </summary>
        /// <param name="id">Yayıncı ID'si.</param>
        /// <returns>Yayıncı bilgisi.</returns>
        Task<DtoPublisherResponse> GetByIdAsync(int id);

        /// <summary>
        /// Yeni bir yayıncı oluşturur.
        /// </summary>
        /// <param name="dto">Yayıncı oluşturma DTO'su.</param>
        /// <returns>Oluşturulan yayıncı bilgisi.</returns>
        Task<DtoPublisherResponse> CreateAsync(DtoPublisherCreate dto);

        /// <summary>
        /// Belirtilen ID'ye sahip yayıncıyı günceller.
        /// </summary>
        /// <param name="id">Yayıncı ID'si.</param>
        /// <param name="dto">Yayıncı güncelleme DTO'su.</param>
        /// <returns>Güncelleme başarılıysa true, değilse false.</returns>
        Task<bool> UpdateAsync(int id, DtoPublisherUpdate dto);

        /// <summary>
        /// Belirtilen ID'ye sahip yayıncıyı siler.
        /// </summary>
        /// <param name="id">Yayıncı ID'si.</param>
        /// <returns>Silme başarılıysa true, değilse false.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// İsme göre yayıncıları arar.
        /// </summary>
        /// <param name="name">Yayıncı adı.</param>
        /// <returns>Arama sonucunda bulunan yayıncılar.</returns>
        Task<IEnumerable<DtoPublisherResponse>> SearchByNameAsync(string name);

        /// <summary>
        /// Yayıncıya ait kitapları getirir.
        /// </summary>
        /// <param name="publisherId">Yayıncı ID'si.</param>
        /// <returns>Yayıncıya ait kitaplar.</returns>
        Task<IEnumerable<DtoBookResponse>> GetBooksByPublisherIdAsync(int publisherId);
    }
}
