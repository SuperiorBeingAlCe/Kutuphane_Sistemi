using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{
    /// <summary>
    /// Admin işlemleri için API controller'ı.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase, IAdminController
    {
        private readonly IAdminService _adminService;

        /// <summary>
        /// AdminController constructor'ı.
        /// </summary>
        /// <param name="adminService">Admin servis bağımlılığı.</param>
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Tüm adminleri getirir.
        /// </summary>
        /// <returns>Admin listesi.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<DtoAdminResponse>>> GetAllAsync()
        {
            var admins = await _adminService.GetAllAsync();
            return Ok(admins);
        }

        /// <summary>
        /// Belirli bir admini id ile getirir.
        /// </summary>
        /// <param name="id">Admin id'si.</param>
        /// <returns>Admin bilgisi.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<DtoAdminResponse?>> GetByIdAsync(int id)
        {
            try
            {
                var admin = await _adminService.GetByIdAsync(id);
                return Ok(admin);
            }
            catch (MyCustomException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Kullanıcı adına göre admin getirir.
        /// </summary>
        /// <param name="username">Kullanıcı adı.</param>
        /// <returns>Admin bilgisi.</returns>
        [HttpGet("username/{username}")]
        public async Task<ActionResult<DtoAdminResponse?>> GetByUsernameAsync(string username)
        {
            try
            {
                var admin = await _adminService.GetByUsernameAsync(username);
                return Ok(admin);
            }
            catch (MyCustomException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Yeni admin oluşturur.
        /// </summary>
        /// <param name="dto">Oluşturulacak admin bilgileri.</param>
        /// <returns>Oluşturulan admin.</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<DtoAdminResponse>> CreateAsync([FromBody] DtoAdminCreate dto)
        {
            var created = await _adminService.CreateAsync(dto);
            if (created == null || created.Id == 0)
            {
                return Ok(created);
            }
            return Created($"api/Admin/{created.Id}", created);
        }

        /// <summary>
        /// Admin girişi yapar.
        /// </summary>
        /// <param name="dto">Giriş bilgileri.</param>
        /// <returns>Giriş sonucu.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<DtoAdminLogin>> LoginAsync([FromBody] DtoAdminLogin dto)
        {
            var isValid = await _adminService.ValidateLoginAsync(dto.Username, dto.Password);
            if (!isValid)
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");
            return Ok(dto);
        }

        /// <summary>
        /// Admin siler.
        /// </summary>
        /// <param name="id">Silinecek admin id'si.</param>
        /// <returns>İşlem sonucu.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var result = await _adminService.DeleteAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
