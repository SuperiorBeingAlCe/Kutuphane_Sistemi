using kitapsin.Server.Dto;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{
    public interface IAdminController
    {
        Task<ActionResult<IEnumerable<DtoAdminResponse>>> GetAllAsync();
        Task<ActionResult<DtoAdminResponse?>> GetByIdAsync(int id);
        Task<ActionResult<DtoAdminResponse?>> GetByUsernameAsync(string username);
        Task<ActionResult<DtoAdminResponse>> CreateAsync(DtoAdminCreate dto);
        Task<ActionResult<DtoAdminLogin>> LoginAsync(DtoAdminLogin dto);
        Task<ActionResult> DeleteAsync(int id);
    }
}
