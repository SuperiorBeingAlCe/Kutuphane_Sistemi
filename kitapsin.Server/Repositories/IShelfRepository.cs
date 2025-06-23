using kitapsin.Server.Models;

namespace kitapsin.Server.Repositories
{
    /// <summary>
    /// Raf işlemleri için repository arayüzü.
    /// </summary>
    public interface IShelfRepository
    {
        /// <summary>
        /// Tüm rafları asenkron olarak getirir.
        /// </summary>
        /// <returns>Tüm rafların listesi.</returns>
        Task<IEnumerable<Shelf>> GetAllAsync();

        /// <summary>
        /// Belirtilen Id'ye sahip rafı asenkron olarak getirir.
        /// </summary>
        /// <param name="id">Raf Id'si.</param>
        /// <returns>Raf nesnesi veya null.</returns>
        Task<Shelf?> GetByIdAsync(int id);

        /// <summary>
        /// Yeni bir raf ekler.
        /// </summary>
        /// <param name="shelf">Eklenecek raf nesnesi.</param>
        Task AddAsync(Shelf shelf);

        /// <summary>
        /// Belirtilen rafa kitap ekler.
        /// </summary>
        /// <param name="shelfId">Raf Id'si.</param>
        /// <param name="bookId">Kitap Id'si.</param>
        Task AddBookIntoShelf(int shelfId, int bookId);

       
        /// <summary>
        /// Rafı siler.
        /// </summary>
        /// <param name="shelf">Silinecek raf nesnesi.</param>
        Task DeleteAsync(Shelf shelf);

        /// <summary>
        /// Değişiklikleri kaydeder.
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        /// Belirtilen raf Id'sine ait tüm kitapları getirir.
        /// </summary>
        /// <param name="ShelfId">Raf Id'si.</param>
        /// <returns>Kitapların listesi.</returns>
        Task<IEnumerable<Book>> GetAllBooksInShelfIdAsync(int ShelfId);
        /// <summary>
        /// Belirtilen rafa ait bir kitabı siler.
        /// </summary>
        /// <param name="shelfId">Raf Id'si.</param>
        /// <param name="bookId">Silinecek kitap Id'si.</param>
        Task RemoveBookFromShelfAsync(int shelfId, int bookId);
    }
}

