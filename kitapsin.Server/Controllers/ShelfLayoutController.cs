using kitapsin.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShelfLayoutController : ControllerBase, IShelfLayoutController
    {
        private readonly IShelfLayoutPreferenceService _service;

        public ShelfLayoutController(IShelfLayoutPreferenceService service)
        {
            _service = service;
        }

        [HttpGet("{adminId}")]
        public async Task<ActionResult<bool?>> GetLayout(int adminId)
        {
            var preference = await _service.GetPreferenceAsync(adminId);
            if (preference == null)
                return NotFound();
            return preference.IsBlockLayout;
        }

        [HttpPost("{adminId}")]
        public async Task<IActionResult> SetLayout(int adminId, [FromBody] bool isBlockLayout)
        {
            await _service.SetPreferenceAsync(adminId, isBlockLayout);
            return Ok();
        }


        // Eksik interface implementasyonları
        [HttpGet]
        public async Task<IActionResult> GetShelfLayoutsAsync()
        {
            // Tüm tercihleri döndürmek için örnek implementasyon
            // Gerçek uygulamada IShelfLayoutPreferenceService'e uygun bir metot eklenmeli
            return StatusCode(501, "Tüm raf düzeni tercihlerini listeleme desteklenmiyor.");
        }

        [HttpGet("layout/{id}")]
        public async Task<IActionResult> GetShelfLayoutByIdAsync(int id)
        {
            var preference = await _service.GetPreferenceAsync(id);
            if (preference == null)
                return NotFound();
            return Ok(preference);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShelfLayoutAsync(int id)
        {
            try
            {
                await _service.DeletePreferenceAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
