using kitapsin.Server.Dto;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Yazarlar ile ilgili işlemleri tanımlar.
    /// </summary>
    public interface IAuthorService
    {
        /// <summary>
        /// Tüm yazarları getirir.
        /// </summary>
        /// <returns>Yazarların listesi.</returns>
        Task<IEnumerable<DtoAuthorResponse>> GetAllAsync();

        /// <summary>
        /// Belirtilen ID'ye sahip yazarı getirir.
        /// </summary>
        /// <param name="id">Yazar ID'si.</param>
        /// <returns>Yazar bilgisi.</returns>
        Task<DtoAuthorResponse> GetByIdAsync(int id);

        /// <summary>
        /// Başlığa göre yazar arar.
        /// </summary>
        /// <param name="title">Aranacak başlık.</param>
        /// <returns>Uygun yazarların listesi.</returns>
        Task<IEnumerable<DtoAuthorResponse>> SearchByTitleAsync(string title);

        /// <summary>
        /// Yeni bir yazar oluşturur.
        /// </summary>
        /// <param name="dto">Yazar oluşturma DTO'su.</param>
        /// <returns>Oluşturulan yazar bilgisi.</returns>
        Task<DtoAuthorResponse> CreateAsync(DtoAuthorCreate dto);

        /// <summary>
        /// Var olan bir yazarı günceller.
        /// </summary>
        /// <param name="id">Yazar ID'si.</param>
        /// <param name="dto">Yazar güncelleme DTO'su.</param>
        /// <returns>Güncelleme başarılıysa true, değilse false.</returns>
        Task<bool> UpdateAsync(int id, DtoAuthorUpdate dto);

        /// <summary>
        /// Belirtilen ID'ye sahip yazarı siler.
        /// </summary>
        /// <param name="id">Yazar ID'si.</param>
        /// <returns>Silme başarılıysa true, değilse false.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Bir yazara ait kitapları getirir.
        /// </summary>
        /// <param name="authorId">Yazar ID'si.</param>
        /// <returns>Yazara ait kitapların listesi.</returns>
        Task<IEnumerable<DtoBookResponse>> GetBooksByAuthorIdAsync(int authorId);
    }
}
