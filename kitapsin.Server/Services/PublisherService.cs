using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Models;
using kitapsin.Server.Repositories;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Yayıncı ile ilgili iş mantığı işlemlerini gerçekleştirir.
    /// </summary>
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _repo;

        /// <summary>
        /// PublisherService sınıfının yeni bir örneğini başlatır.
        /// </summary>
        /// <param name="repo">Yayıncı repository bağımlılığı.</param>
        public PublisherService(IPublisherRepository repo)
        {
            _repo = repo;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DtoPublisherResponse>> GetAllAsync()
        {
            var publishers = await _repo.GetAllAsync();
            return publishers.Select(p => new DtoPublisherResponse
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                Phone = p.Phone,
                Email = p.Email
            });
        }

        /// <inheritdoc/>
        public async Task<DtoPublisherResponse> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new MyCustomException("Geçersiz yayıncı Id.");

            var publisher = await _repo.GetByIdAsync(id);
            if (publisher == null)
                throw new MyCustomException($"Yayıncı bulunamadı. Id={id}");

            return new DtoPublisherResponse
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Address = publisher.Address,
                Phone = publisher.Phone,
                Email = publisher.Email
            };
        }

        /// <inheritdoc/>
        public async Task<DtoPublisherResponse> CreateAsync(DtoPublisherCreate dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new MyCustomException("Yayıncı adı boş olamaz.");
            if (string.IsNullOrWhiteSpace(dto.Address))
                throw new MyCustomException("Adres boş olamaz.");
            if (string.IsNullOrWhiteSpace(dto.Phone))
                throw new MyCustomException("Telefon boş olamaz.");
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new MyCustomException("E-posta boş olamaz.");

            var publisher = new Publisher
            {
                Name = dto.Name.Trim(),
                Address = dto.Address.Trim(),
                Phone = dto.Phone.Trim(),
                Email = dto.Email.Trim()
            };

            await _repo.AddAsync(publisher);
            await _repo.SaveChangesAsync();

            return new DtoPublisherResponse
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Address = publisher.Address,
                Phone = publisher.Phone,
                Email = publisher.Email
            };
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(int id, DtoPublisherUpdate dto)
        {
            if (id <= 0)
                throw new MyCustomException("Geçersiz yayıncı Id.");

            var publisher = await _repo.GetByIdAsync(id);
            if (publisher == null)
                throw new MyCustomException($"Güncellenecek yayıncı bulunamadı. Id={id}");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new MyCustomException("Yayıncı adı boş olamaz.");

            publisher.Name = dto.Name.Trim();

            await _repo.UpdateAsync(publisher);
            await _repo.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new MyCustomException("Geçersiz yayıncı Id.");

            var publisher = await _repo.GetByIdAsync(id);
            if (publisher == null)
                throw new MyCustomException($"Silinecek yayıncı bulunamadı. Id={id}");

            await _repo.DeleteAsync(publisher);
            await _repo.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DtoBookResponse>> GetBooksByPublisherIdAsync(int publisherId)
        {
            if (publisherId <= 0)
                throw new MyCustomException("Geçersiz yayıncı Id.");

            var books = await _repo.GetBooksByPublisherIdAsync(publisherId);
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

        /// <inheritdoc/>
        public async Task<IEnumerable<DtoPublisherResponse>> SearchByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new MyCustomException("Yayıncı adı boş olamaz.");

            var publishers = await _repo.SearchByNameAsync(name);
            return publishers.Select(p => new DtoPublisherResponse
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                Phone = p.Phone,
                Email = p.Email
            });
        }
    }
}
