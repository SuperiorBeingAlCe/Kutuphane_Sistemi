using kitapsin.Server.Dto;
using kitapsin.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShelfController : ControllerBase, IShelfController
    {
        private readonly IShelfService _shelfService;

        public ShelfController(IShelfService shelfService)
        {
            _shelfService = shelfService;
        }

        /// <summary>
        /// Tüm rafları getirir.
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<DtoShelf>> GetAllAsync()
        {
            return await _shelfService.GetAllAsync();
        }

        /// <summary>
        /// Belirtilen Id'ye sahip rafı getirir.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<DtoShelf?> GetByIdAsync(int id)
        {
            return await _shelfService.GetByIdAsync(id);
        }

        /// <summary>
        /// Yeni raf ekler.
        /// </summary>
        [HttpPost]
        public async Task AddAsync([FromBody] DtoShelf dto)
        {
            await _shelfService.AddAsync(dto);
        }

        /// <summary>
        /// Bir kitabı rafa ekler.
        /// </summary>
        [HttpPost("{shelfId}/books/{bookId}")]
        public async Task AddBookIntoShelf(int shelfId, int bookId)
        {
            await _shelfService.AddBookIntoShelf(shelfId, bookId);
        }

        /// <summary>
        /// Belirtilen rafı siler.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            await _shelfService.DeleteAsync(id);
        }

        /// <summary>
        /// Belirtilen rafın kitaplarını getirir.
        /// </summary>
        [HttpGet("{shelfId}/books")]
        public async Task<IEnumerable<DtoBookResponse>> GetAllBooksInShelfIdAsync(int shelfId)
        {
            return await _shelfService.GetAllBooksInShelfIdAsync(shelfId);
        }

        /// <summary>
        /// Belirtilen rafa ait kitabı siler.
        /// </summary>
        [HttpDelete("{shelfId}/books/{bookId}")]
        public async Task RemoveBookFromShelfAsync(int shelfId, int bookId)
        {
            await _shelfService.RemoveBookFromShelfAsync(shelfId, bookId);
        }

    }
}
