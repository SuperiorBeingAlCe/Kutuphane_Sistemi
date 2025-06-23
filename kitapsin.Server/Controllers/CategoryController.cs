using kitapsin.Server.Dto;
using kitapsin.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase, ICategoryController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<DtoCategoryResponse>> GetAllAsync()
        {
            return await _categoryService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<DtoCategoryResponse?> GetByIdAsync(int id)
        {
            return await _categoryService.GetByIdAsync(id);
        }

        [HttpGet("search")]
        public async Task<IEnumerable<DtoCategoryResponse>> SearchByNameAsync([FromQuery] string title)
        {
            return await _categoryService.SearchByNameAsync(title);
        }

        [HttpPost]
        public async Task<DtoCategoryResponse> CreateAsync([FromBody] DtoCategoryCreate dto)
        {
            return await _categoryService.CreateAsync(dto);
        }

        [HttpPut("{id}")]
        public async Task<bool> UpdateAsync(int id, [FromBody] DtoCategoryUpdate dto)
        {
            return await _categoryService.UpdateAsync(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteAsync(int id)
        {
            return await _categoryService.DeleteAsync(id);
        }

        [HttpGet("{categoryId}/books")]
        public async Task<IEnumerable<DtoBookResponse>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _categoryService.GetBooksByCategoryAsync(categoryId);
        }
    }
}
