using kitapsin.Server.Dto;
using kitapsin.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase, IAuthController
    {
        private readonly IAuthorizationService _authService;

        public AuthController(IAuthorizationService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DtoAdminLogin dto)
        {
            var token = await _authService.LoginAsync(dto);
            if (token == null)
                return Unauthorized("Kullanıcı adı veya şifre hatalı.");

            return Ok(new { Token = token });
        }
    }
}
