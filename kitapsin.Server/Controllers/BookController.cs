using kitapsin.Server.Dto;
using kitapsin.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase, IBookController
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<DtoBookResponse>>> GetAll()
        {
            var result = await _bookService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DtoBookResponse>> GetById(int id)
        {
            var result = await _bookService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DtoBookResponse>>> SearchByTitle([FromQuery] string title)
        {
            var result = await _bookService.SearchByTitleAsync(title);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<DtoBookResponse>> Create([FromBody] DtoBookCreate dto)
        {
            var result = await _bookService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DtoBookUpdate dto)
        {
            var success = await _bookService.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _bookService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
