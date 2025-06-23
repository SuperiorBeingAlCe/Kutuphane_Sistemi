using kitapsin.Server.Dto;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{
    public interface IAuthController
    {
        Task<IActionResult> Login([FromBody] DtoAdminLogin dto);
    }
}
