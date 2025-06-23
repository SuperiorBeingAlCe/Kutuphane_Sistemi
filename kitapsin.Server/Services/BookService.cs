using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Models;
using kitapsin.Server.Repositories;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Kitap işlemlerini yöneten servis sınıfı.
    /// </summary>
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPublisherRepository _publisherRepository;

        /// <summary>
        /// BookService sınıfının kurucusu.
        /// </summary>
        /// <param name="bookRepository">Kitap repository bağımlılığı.</param>
        /// <param name="authorRepository">Yazar repository bağımlılığı.</param>
        /// <param name="categoryRepository">Kategori repository bağımlılığı.</param>
        /// <param name="publisherRepository">Yayınevi repository bağımlılığı.</param>
        public BookService(
            IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository,
            IPublisherRepository publisherRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _publisherRepository = publisherRepository;
        }

        /// <summary>
        /// Tüm kitapları getirir.
        /// </summary>
        /// <returns>Kitapların DTO listesi.</returns>
        /// <exception cref="MyCustomException">Hiç kitap yoksa fırlatılır.</exception>
        public async Task<IEnumerable<DtoBookResponse>> GetAllAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            if (!books.Any())
                throw new MyCustomException("Herhangi bir kitap bulunamadı.");

            return books.Select(b => new DtoBookResponse
            {
                Id = b.Id,
                Title = b.Title,
                AuthorId = b.AuthorId,
                CategoryId = b.CategoryId,
                PublisherId = b.PublisherId,
                PublicationYear = b.PublicationYear,
                ISBN = b.ISBN,
                Quantity = b.Quantity,
                CreatedAt = b.CreatedAt,
                IsActive = b.IsActive
            });
        }

        /// <summary>
        /// Belirtilen Id'ye sahip kitabı getirir.
        /// </summary>
        /// <param name="id">Kitap Id'si.</param>
        /// <returns>Kitap DTO'su.</returns>
        /// <exception cref="MyCustomException">Kitap bulunamazsa fırlatılır.</exception>
        public async Task<DtoBookResponse> GetByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                throw new MyCustomException($"Kitap bulunamadı. Id={id}");

            return new DtoBookResponse
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                PublisherId = book.PublisherId,
                PublicationYear = book.PublicationYear,
                ISBN = book.ISBN,
                Quantity = book.Quantity,
                CreatedAt = book.CreatedAt,
                IsActive = book.IsActive
            };
        }

        /// <summary>
        /// Başlığa göre kitap arar.
        /// </summary>
        /// <param name="title">Kitap başlığı.</param>
        /// <returns>Uygun kitapların DTO listesi.</returns>
        /// <exception cref="MyCustomException">Hiç kitap bulunamazsa fırlatılır.</exception>
        public async Task<IEnumerable<DtoBookResponse>> SearchByTitleAsync(string title)
        {
            var books = await _bookRepository.SearchByTitleAsync(title);
            if (!books.Any())
                throw new MyCustomException($"'{title}' başlıklı kitap bulunamadı.");

            return books.Select(b => new DtoBookResponse
            {
                Id = b.Id,
                Title = b.Title,
                AuthorId = b.AuthorId,
                CategoryId = b.CategoryId,
                PublisherId = b.PublisherId,
                PublicationYear = b.PublicationYear,
                ISBN = b.ISBN,
                Quantity = b.Quantity,
                CreatedAt = b.CreatedAt,
                IsActive = b.IsActive
            });
        }

        /// <summary>
        /// Yeni bir kitap oluşturur.
        /// </summary>
        /// <param name="dto">Kitap oluşturma DTO'su.</param>
        /// <returns>Oluşturulan kitabın DTO'su.</returns>
        /// <exception cref="MyCustomException">Geçersiz veri veya ilişkili nesne bulunamazsa fırlatılır.</exception>
        public async Task<DtoBookResponse> CreateAsync(DtoBookCreate dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new MyCustomException("Geçersiz kitap başlığı.");

            var author = await _authorRepository.GetByIdAsync(dto.AuthorId);
            if (author == null)
                throw new MyCustomException($"Yazar bulunamadı. AuthorId={dto.AuthorId}");

            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null)
                throw new MyCustomException($"Kategori bulunamadı. CategoryId={dto.CategoryId}");

            var publisher = await _publisherRepository.GetByIdAsync(dto.PublisherId);
            if (publisher == null)
                throw new MyCustomException($"Yayınevi bulunamadı. PublisherId={dto.PublisherId}");

            var book = new Book
            {
                Title = dto.Title,
                AuthorId = dto.AuthorId,
                CategoryId = dto.CategoryId,
                PublisherId = dto.PublisherId,
                PublicationYear = dto.PublicationYear,
                ISBN = dto.ISBN,
                Quantity = dto.Quantity,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();

            var created = await _bookRepository.GetByIdAsync(book.Id)
                ?? throw new MyCustomException("Kaydedilen kitap bulunamadı.");

            return new DtoBookResponse
            {
                Id = created.Id,
                Title = created.Title,
                AuthorId = created.AuthorId,
                CategoryId = created.CategoryId,
                PublisherId = created.PublisherId,
                PublicationYear = created.PublicationYear,
                ISBN = created.ISBN,
                Quantity = created.Quantity,
                CreatedAt = created.CreatedAt,
                IsActive = created.IsActive
            };
        }

        /// <summary>
        /// Var olan bir kitabı günceller.
        /// </summary>
        /// <param name="id">Kitap Id'si.</param>
        /// <param name="dto">Güncelleme DTO'su.</param>
        /// <returns>Başarılıysa true.</returns>
        /// <exception cref="MyCustomException">Kitap veya ilişkili nesne bulunamazsa fırlatılır.</exception>
        public async Task<bool> UpdateAsync(int id, DtoBookUpdate dto)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                throw new MyCustomException($"Güncellenecek kitap bulunamadı. Id={id}");

            if (await _authorRepository.GetByIdAsync(dto.AuthorId) == null)
                throw new MyCustomException($"Kitap için yazar bulunamadı. AuthorId={dto.AuthorId}");
            if (await _categoryRepository.GetByIdAsync(dto.CategoryId) == null)
                throw new MyCustomException($"Kitap için kategori bulunamadı. CategoryId={dto.CategoryId}");

            book.Title = dto.Title;
            book.AuthorId = dto.AuthorId;
            book.CategoryId = dto.CategoryId;
            book.PublisherId = dto.PublisherId;
            book.PublicationYear = dto.PublicationYear;
            book.ISBN = dto.ISBN;
            book.Quantity = dto.Quantity;
            book.IsActive = dto.IsActive;

            await _bookRepository.UpdateAsync(book);
            await _bookRepository.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Bir kitabı siler.
        /// </summary>
        /// <param name="id">Kitap Id'si.</param>
        /// <returns>Başarılıysa true.</returns>
        /// <exception cref="MyCustomException">Kitap bulunamazsa fırlatılır.</exception>
        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                throw new MyCustomException($"Silinecek kitap bulunamadı. Id={id}");

            await _bookRepository.DeleteAsync(book);
            await _bookRepository.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Bir kitabın yazarını değiştirir.
        /// </summary>
        /// <param name="bookId">Kitap Id'si.</param>
        /// <param name="newAuthorId">Yeni yazar Id'si.</param>
        /// <returns>Task</returns>
        /// <exception cref="MyCustomException">Kitap veya yazar bulunamazsa ya da yazar zaten atanmışsa fırlatılır.</exception>
        public async Task ChangeBookAuthorAsync(int bookId, int newAuthorId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                throw new MyCustomException("kitap bulunamadı!");

            if (book.AuthorId == newAuthorId)
                throw new MyCustomException("kitap bir yazara ait zaten!");

            var author = await _authorRepository.GetByIdAsync(newAuthorId);
            if (author == null)
                throw new MyCustomException("yazar bulunamadı!");

            book.AuthorId = newAuthorId;
            await _bookRepository.UpdateAsync(book);
            await _bookRepository.SaveChangesAsync();
        }
    }
    }

