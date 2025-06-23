using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace kitapsin.Server.Services
{
    /// <summary>
    /// Kategori işlemleri için servis.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// CategoryService sınıfının kurucusu.
        /// </summary>
        /// <param name="context">Veritabanı bağlamı</param>
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Tüm kategorileri getirir.
        /// </summary>
        /// <returns>Kategori yanıtlarının listesi</returns>
        public async Task<IEnumerable<DtoCategoryResponse>> GetAllAsync()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .Select(c => new DtoCategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            return categories;
        }

        /// <summary>
        /// Id'ye göre kategori getirir.
        /// </summary>
        /// <param name="id">Kategori Id</param>
        /// <returns>Kategori yanıtı</returns>
        /// <exception cref="MyCustomException">Kategori bulunamazsa fırlatılır</exception>
        public async Task<DtoCategoryResponse> GetByIdAsync(int id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new DtoCategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .FirstOrDefaultAsync();

            if (category == null)
                throw new MyCustomException($"Kategori bulunamadı. Id={id}");

            return category;
        }

        /// <summary>
        /// Kategori adına göre arama yapar.
        /// </summary>
        /// <param name="title">Kategori adı</param>
        /// <returns>Kategori yanıtlarının listesi</returns>
        /// <exception cref="MyCustomException">Kategori adı boşsa fırlatılır</exception>
        public async Task<IEnumerable<DtoCategoryResponse>> SearchByNameAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new MyCustomException("Kategori adı boş olamaz.");

            var categories = await _context.Categories
                .AsNoTracking()
                .Where(c => c.Name.Contains(title))
                .Select(c => new DtoCategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            return categories;
        }

        /// <summary>
        /// Yeni kategori oluşturur.
        /// </summary>
        /// <param name="dto">Kategori oluşturma DTO'su</param>
        /// <returns>Oluşturulan kategori yanıtı</returns>
        /// <exception cref="MyCustomException">İstek veya isim zaten varsa fırlatılır</exception>
        public async Task<DtoCategoryResponse> CreateAsync(DtoCategoryCreate dto)
        {
            if (dto == null)
                throw new MyCustomException("Kategori oluşturma isteği boş olamaz.");

            var exists = await _context.Categories.AnyAsync(c => c.Name == dto.Name);
            if (exists)
                throw new MyCustomException("Bu isimde bir kategori zaten mevcut.");

            var category = new Category
            {
                Name = dto.Name,
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new DtoCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        /// <summary>
        /// Kategori günceller.
        /// </summary>
        /// <param name="id">Kategori Id</param>
        /// <param name="dto">Kategori güncelleme DTO'su</param>
        /// <returns>Başarılıysa true</returns>
        /// <exception cref="MyCustomException">İstek veya kategori yoksa fırlatılır</exception>
        public async Task<bool> UpdateAsync(int id, DtoCategoryUpdate dto)
        {
            if (dto == null)
                throw new MyCustomException("Kategori güncelleme isteği boş olamaz.");

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new MyCustomException("Kategori bulunamadı.");

            category.Name = dto.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Kategori siler.
        /// </summary>
        /// <param name="id">Kategori Id</param>
        /// <returns>Başarılıysa true</returns>
        /// <exception cref="MyCustomException">Kategori yoksa fırlatılır</exception>
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new MyCustomException("Kategori bulunamadı.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Kategoriye ait kitapları getirir.
        /// </summary>
        /// <param name="categoryId">Kategori Id</param>
        /// <returns>Kitap yanıtlarının listesi</returns>
        public async Task<IEnumerable<DtoBookResponse>> GetBooksByCategoryAsync(int categoryId)
        {
            var books = await _context.Books
                .AsNoTracking()
                .Where(b => b.CategoryId == categoryId)
                .Select(b => new DtoBookResponse
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
                })
                .ToListAsync();

            return books;
        }
    }
}
