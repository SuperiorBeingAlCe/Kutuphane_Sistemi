using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Models;
using kitapsin.Server.Repositories;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Raf işlemleri için servis sınıfı.
    /// </summary>
    public class ShelfService : IShelfService
    {
        private readonly IShelfRepository _repo;

        /// <summary>
        /// ShelfService sınıfının kurucusu.
        /// </summary>
        /// <param name="repo">Raf repository bağımlılığı.</param>
        public ShelfService(IShelfRepository repo)
        {
            _repo = repo;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DtoShelf>> GetAllAsync()
        {
            var shelves = await _repo.GetAllAsync();

            return shelves.Select(s => new DtoShelf
            {
                Section = s.Section,
                Row = s.Row,
                Column = s.Column
            });
        }

        /// <inheritdoc/>
        public async Task<DtoShelf?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new MyCustomException("Geçersiz raf Id.");

            var shelf = await _repo.GetByIdAsync(id);
            if (shelf == null)
                return null;

            return new DtoShelf
            {
                Section = shelf.Section,
                Row = shelf.Row,
                Column = shelf.Column
            };
        }

        /// <inheritdoc/>
        public async Task AddAsync(DtoShelf dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Section))
                throw new MyCustomException("Bölüm (section) boş olamaz.");

            if (dto.Row <= 0 || dto.Column <= 0)
                throw new MyCustomException("Satır ve sütun sayısı pozitif olmalıdır.");

            var shelf = new Shelf
            {
                Section = dto.Section.Trim(),
                Row = dto.Row,
                Column = dto.Column,
                Books = new List<Book>()
            };

            await _repo.AddAsync(shelf);
            await _repo.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new MyCustomException("Geçersiz raf Id.");

            var shelf = await _repo.GetByIdAsync(id);
            if (shelf == null)
                throw new MyCustomException($"Silinecek raf bulunamadı. Id={id}");

            await _repo.DeleteAsync(shelf);
            await _repo.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task AddBookIntoShelf(int shelfId, int bookId)
        {
            if (shelfId <= 0 || bookId <= 0)
                throw new MyCustomException("Geçersiz raf veya kitap Id.");

            var shelf = await _repo.GetByIdAsync(shelfId);
            if (shelf == null)
                throw new MyCustomException($"Raf bulunamadı. Id={shelfId}");

            await _repo.AddBookIntoShelf(shelfId, bookId);
            await _repo.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task RemoveBookFromShelfAsync(int shelfId, int bookId)
        {
            if (shelfId <= 0 || bookId <= 0)
                throw new MyCustomException("Geçersiz raf veya kitap Id.");

            var shelf = await _repo.GetByIdAsync(shelfId);
            if (shelf == null)
                throw new MyCustomException($"Raf bulunamadı. Id={shelfId}");

            await _repo.RemoveBookFromShelfAsync(shelfId, bookId);
            await _repo.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DtoBookResponse>> GetAllBooksInShelfIdAsync(int shelfId)
        {
            if (shelfId <= 0)
                throw new MyCustomException("Geçersiz raf Id.");

            var books = await _repo.GetAllBooksInShelfIdAsync(shelfId);
            return books.Select(b => new DtoBookResponse
            {
                Id = b.Id,
                Title = b.Title,
                AuthorId = b.AuthorId,
                PublisherId = b.PublisherId,
                PublicationYear = b.PublicationYear,
                ISBN = b.ISBN,
                Quantity = b.Quantity,
                CreatedAt = b.CreatedAt,
                IsActive = b.IsActive,
                CategoryId = b.CategoryId
            });
        }
    }
}
