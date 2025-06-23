using kitapsin.Server.Dto;
using kitapsin.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PublisherController : ControllerBase, IPublisherController
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        /// <summary>
        /// Tüm yayınevlerini listeler.
        /// </summary>
        /// <returns>Yayınevlerinin listesi.</returns>
        [HttpGet]
        public async Task<IEnumerable<DtoPublisherResponse>> GetAllAsync()
        {
            // Yayınevlerini getirir
            return await _publisherService.GetAllAsync();
        }


        [HttpGet("{id}")]
        public async Task<DtoPublisherResponse> GetByIdAsync(int id)
        {
            return await _publisherService.GetByIdAsync(id);
        }

        [HttpGet("search")]
        public async Task<IEnumerable<DtoPublisherResponse>> SearchByNameAsync([FromQuery] string name)
        {
            return await _publisherService.SearchByNameAsync(name);
        }

        /// <summary>
        /// Yeni bir yayınevi ekler.
        /// </summary>
        /// <param name="publisher">Eklenecek yayınevi nesnesi.</param>
        /// <returns>Eklenen yayınevi bilgisi.</returns>
        [HttpPost]
        public async Task<DtoPublisherResponse> CreateAsync([FromBody] DtoPublisherCreate dto)
        {
            // Yayınevi ekler
            return await _publisherService.CreateAsync(dto);
        }

        [HttpPut("{id}")]
        public async Task<bool> UpdateAsync(int id, [FromBody] DtoPublisherUpdate dto)
        {
            return await _publisherService.UpdateAsync(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteAsync(int id)
        {
            return await _publisherService.DeleteAsync(id);
        }

        [HttpGet("{publisherId}/books")]
        public async Task<IEnumerable<DtoBookResponse>> GetBooksByPublisherIdAsync(int publisherId)
        {
            return await _publisherService.GetBooksByPublisherIdAsync(publisherId);
        }
    }
}
