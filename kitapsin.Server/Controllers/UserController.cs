using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Models;
using kitapsin.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase, IUserController
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<DtoUserResponse>>> GetAllAsync()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DtoUserResponse?>> GetByIdAsync(int id)
        {
            try
            {
                var user = await _service.GetByIdAsync(id);
                return Ok(user);
            }
            catch (MyCustomException ex)
            {
                return NotFound(ex.Message); 
            }
        }

        [HttpGet("card/{cardNumber}")]
        public async Task<ActionResult<DtoUserResponse?>> GetByCardNumberAsync(string cardNumber)
        {
            try
            {
                var user = await _service.GetByCardNumberAsync(cardNumber);
                return Ok(user);
            }
            catch (MyCustomException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DtoUserResponse>>> SearchByNameAsync([FromQuery] string name)
        {
            var users = await _service.SearchByNameAsync(name);
            return Ok(users);
        }

        [HttpPost("add")]
        public async Task<ActionResult<DtoUserResponse>> CreateAsync(DtoUserCreate dto)
        {
            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, DtoUserUpdate dto)
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

        [HttpGet("{userId}/loans")]
        public async Task<ActionResult<IEnumerable<DtoLoanResponse>>> GetLoansAsync(int userId)
        {
            var loans = await _service.GetLoansAsync(userId);
            return Ok(loans);
        }

        [HttpGet("{userId}/penalties")]
        public async Task<ActionResult<IEnumerable<DtoPenaltyResponse>>> GetPenaltiesAsync(int userId)
        {
            var penalties = await _service.GetPenaltiesAsync(userId);
            return Ok(penalties);
        }

        [HttpGet("{userId}/borrowed-books")]
        public async Task<ActionResult<IEnumerable<DtoBookResponse>>> GetBorrowedBooksAsync(int userId)
        {
            var books = await _service.GetBorrowedBooksAsync(userId);
            return Ok(books);
        }
    }
}
