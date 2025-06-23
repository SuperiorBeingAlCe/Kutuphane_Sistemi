using kitapsin.Server.Dto;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Kategori ile ilgili işlemleri tanımlar.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Tüm kategorileri getirir.
        /// </summary>
        /// <returns>Tüm kategorilerin listesi.</returns>
        Task<IEnumerable<DtoCategoryResponse>> GetAllAsync();

        /// <summary>
        /// Belirli bir kategori bilgisini getirir.
        /// </summary>
        /// <param name="id">Kategori Id'si.</param>
        /// <returns>Kategori bilgisi.</returns>
        Task<DtoCategoryResponse> GetByIdAsync(int id);

        /// <summary>
        /// Kategori adına göre arama yapar.
        /// </summary>
        /// <param name="title">Kategori adı.</param>
        /// <returns>Arama sonucunda bulunan kategoriler.</returns>
        Task<IEnumerable<DtoCategoryResponse>> SearchByNameAsync(string title);

        /// <summary>
        /// Yeni bir kategori oluşturur.
        /// </summary>
        /// <param name="dto">Kategori oluşturma DTO'su.</param>
        /// <returns>Oluşturulan kategori.</returns>
        Task<DtoCategoryResponse> CreateAsync(DtoCategoryCreate dto);

        /// <summary>
        /// Kategori bilgisini günceller.
        /// </summary>
        /// <param name="id">Kategori Id'si.</param>
        /// <param name="dto">Kategori güncelleme DTO'su.</param>
        /// <returns>Güncelleme başarılıysa true, değilse false.</returns>
        Task<bool> UpdateAsync(int id, DtoCategoryUpdate dto);

        /// <summary>
        /// Kategoriyi siler.
        /// </summary>
        /// <param name="id">Kategori Id'si.</param>
        /// <returns>Silme başarılıysa true, değilse false.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Kategoriye ait kitapları getirir.
        /// </summary>
        /// <param name="authorId">Kategori Id'si.</param>
        /// <returns>Kategoriye ait kitaplar.</returns>
        Task<IEnumerable<DtoBookResponse>> GetBooksByCategoryAsync(int authorId);
    }
}
