using kitapsin.Server.Dto;
using kitapsin.Server.Models;
using kitapsin.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : ControllerBase, ILoanController
    {
        private readonly ILoanService _service;

        public LoanController(ILoanService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpPost]
        public async Task<ActionResult<DtoLoanResponse>> AddAsync([FromBody] DtoLoanCreate dto)
        {
            var created = await _service.AddAsync(dto);
            return Ok(created);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DtoLoanResponse>> GetByIdAsync(int id)
        {
            var loan = await _service.GetByIdAsync(id);
            if (loan == null)
                return NotFound();
            return Ok(loan);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DtoLoanResponse>>> GetAllAsync()
        {
            var loans = await _service.GetAllAsync();
            return Ok(loans);
        }
    }

    }
