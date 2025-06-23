using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Models;
using kitapsin.Server.Repositories;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Yazar işlemleri için servis katmanı.
    /// </summary>
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repo;

        /// <summary>
        /// AuthorService sınıfı için yeni bir örnek oluşturur.
        /// </summary>
        /// <param name="repo">Yazar repository bağımlılığı.</param>
        public AuthorService(IAuthorRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Tüm yazarları getirir.
        /// </summary>
        /// <returns>Yazarların DTO listesi.</returns>
        public async Task<IEnumerable<DtoAuthorResponse>> GetAllAsync()
        {
            var authors = await _repo.GetAllAsync();
            return authors.Select(a => new DtoAuthorResponse
            {
                Id = a.Id,
                Name = a.Name
            });
        }

        /// <summary>
        /// Id'ye göre yazar getirir.
        /// </summary>
        /// <param name="id">Yazar Id'si.</param>
        /// <returns>Yazar DTO'su.</returns>
        /// <exception cref="MyCustomException">Yazar bulunamazsa fırlatılır.</exception>
        public async Task<DtoAuthorResponse> GetByIdAsync(int id)
        {
            var author = await _repo.GetByIdAsync(id);
            if (author == null)
                throw new MyCustomException($"Author bulunamadı. Id={id}");

            return new DtoAuthorResponse
            {
                Id = author.Id,
                Name = author.Name,

            };
        }

        /// <summary>
        /// Başlığa göre yazar arar.
        /// </summary>
        /// <param name="title">Aranacak başlık.</param>
        /// <returns>Yazarların DTO listesi.</returns>
        /// <exception cref="MyCustomException">Yazar bulunamazsa fırlatılır.</exception>
        public async Task<IEnumerable<DtoAuthorResponse>> SearchByTitleAsync(string title)
        {
            var authors = await _repo.SearchByTitleAsync(title);
            if (!authors.Any())
                throw new MyCustomException($"'{title}' içeren yazar bulunamadı.");

            return authors.Select(a => new DtoAuthorResponse
            {
                Id = a.Id,
                Name = a.Name
            });
        }

        /// <summary>
        /// Yeni yazar oluşturur.
        /// </summary>
        /// <param name="dto">Yazar oluşturma DTO'su.</param>
        /// <returns>Oluşturulan yazar DTO'su.</returns>
        /// <exception cref="MyCustomException">Aynı isimde yazar varsa fırlatılır.</exception>
        public async Task<DtoAuthorResponse> CreateAsync(DtoAuthorCreate dto)
        {
            if ((await _repo.SearchByTitleAsync(dto.Name)).Any(a =>
                     string.Equals(a.Name, dto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new MyCustomException($"Bu isimde zaten bir yazar var: '{dto.Name}'");
            }

            var newAuthor = new Author
            {
                Name = dto.Name
            };

            await _repo.AddAsync(newAuthor);
            await _repo.SaveChangesAsync();

            return new DtoAuthorResponse
            {
                Id = newAuthor.Id,
                Name = newAuthor.Name
            };
        }

        /// <summary>
        /// Yazar günceller.
        /// </summary>
        /// <param name="id">Yazar Id'si.</param>
        /// <param name="dto">Yazar güncelleme DTO'su.</param>
        /// <returns>Başarılıysa true.</returns>
        /// <exception cref="MyCustomException">Yazar bulunamazsa fırlatılır.</exception>
        public async Task<bool> UpdateAsync(int id, DtoAuthorUpdate dto)
        {
            var author = await _repo.GetByIdAsync(id);
            if (author == null)
                throw new MyCustomException($"Güncellenecek yazar bulunamadı. Id={id}");

            author.Name = dto.Name;

            await _repo.UpdateAsync(author);
            await _repo.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Yazar siler.
        /// </summary>
        /// <param name="id">Yazar Id'si.</param>
        /// <returns>Başarılıysa true.</returns>
        /// <exception cref="MyCustomException">Yazar bulunamazsa fırlatılır.</exception>
        public async Task<bool> DeleteAsync(int id)
        {
            var author = await _repo.GetByIdAsync(id);
            if (author == null)
                throw new MyCustomException($"Silinecek yazar bulunamadı. Id={id}");

            await _repo.DeleteAsync(author);
            await _repo.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Yazar Id'sine göre kitapları getirir.
        /// </summary>
        /// <param name="authorId">Yazar Id'si.</param>
        /// <returns>Kitapların DTO listesi.</returns>
        /// <exception cref="MyCustomException">Yazar veya kitap bulunamazsa fırlatılır.</exception>
        public async Task<IEnumerable<DtoBookResponse>> GetBooksByAuthorIdAsync(int authorId)
        {
            var author = await _repo.GetByIdAsync(authorId);
            if (author == null)
                throw new MyCustomException($"Yazar bulunamadı. Id={authorId}");

            var books = await _repo.GetBooksByAuthorIdAsync(authorId);
            if (!books.Any())
                throw new MyCustomException($"Bu yazara ait kitap bulunamadı. AuthorId={authorId}");

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
    }
}