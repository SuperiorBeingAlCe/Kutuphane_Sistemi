using kitapsin.Server.Dto;

namespace kitapsin.Server.Controllers
{
    public interface IShelfController
    {
        /// <summary>
        /// Tüm rafları asenkron olarak getirir.
        /// </summary>
        /// <returns>Tüm rafların listesi.</returns>
        Task<IEnumerable<DtoShelf>> GetAllAsync();

        /// <summary>
        /// Belirtilen id'ye sahip rafı getirir.
        /// </summary>
        /// <param name="id">Raf id'si.</param>
        /// <returns>Raf nesnesi veya null.</returns>
        Task<DtoShelf?> GetByIdAsync(int id);

        /// <summary>
        /// Yeni bir raf ekler.
        /// </summary>
        /// <param name="dto">Eklenecek raf DTO'su.</param>
        Task AddAsync(DtoShelf dto);

        /// <summary>
        /// Bir kitabı belirtilen rafa ekler.
        /// </summary>
        /// <param name="shelfId">Raf id'si.</param>
        /// <param name="bookId">Kitap id'si.</param>
        Task AddBookIntoShelf(int shelfId, int bookId);

        /// <summary>
        /// Belirtilen id'ye sahip rafı siler.
        /// </summary>
        /// <param name="id">Raf id'si.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Belirtilen rafa ait tüm kitapları getirir.
        /// </summary>
        /// <param name="shelfId">Raf id'si.</param>
        /// <returns>Kitapların listesi.</returns>
        Task<IEnumerable<DtoBookResponse>> GetAllBooksInShelfIdAsync(int shelfId);

        /// <summary>
        /// Bir kitabı rafdan kaldırır.
        /// </summary>
        /// <param name="shelfId">Raf id'si.</param>
        /// <param name="bookId">Kitap id'si.</param>
        Task RemoveBookFromShelfAsync(int shelfId, int bookId);
    }
}

