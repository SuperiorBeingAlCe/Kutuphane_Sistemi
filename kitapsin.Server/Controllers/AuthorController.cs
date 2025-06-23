using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase, IAuthorController
    {
        private readonly IAuthorService _service;

        public AuthorController(IAuthorService service)
        {
            _service = service;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<DtoAuthorResponse>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DtoAuthorResponse?>> GetByIdAsync(int id)
        {
            var author = await _service.GetByIdAsync(id);
            if (author == null)
                return NotFound();

            return Ok(author);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DtoAuthorResponse>>> SearchByTitleAsync([FromQuery] string title)
        {
            var result = await _service.SearchByTitleAsync(title);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<DtoAuthorResponse>> CreateAsync([FromBody] DtoAuthorCreate dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return Ok(created);
            }
            catch (MyCustomException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] DtoAuthorUpdate dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpGet("{authorId}/books")]
        public async Task<ActionResult<IEnumerable<DtoBookResponse>>> GetBooksByAuthorIdAsync(int authorId)
        {
            var result = await _service.GetBooksByAuthorIdAsync(authorId);
            return Ok(result);
        }
    }
}
