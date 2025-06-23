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
    public class PenaltyController : ControllerBase, IPenaltyController
    {
        private readonly IPenaltyService _service;
        public PenaltyController(IPenaltyService service) => _service = service;

        [HttpPost]
        public async Task<ActionResult<DtoPenaltyResponse>> AddAsync([FromBody] DtoPenaltyCreate dto)
        {
            var created = await _service.AddAsync(dto);
            return Ok(created);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> PayAndRemoveAsync(int id)
        {
            var success = await _service.PayAndRemoveAsync(id);
            return success ? NoContent() : NotFound();
        }
    }

    }

