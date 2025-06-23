using kitapsin.Server.Dto;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{
    public interface ILoanController
    {
        Task<ActionResult<DtoLoanResponse>> AddAsync(DtoLoanCreate dto);
        Task<ActionResult> DeleteAsync(int id);
        Task<ActionResult<DtoLoanResponse>> GetByIdAsync(int id);
        Task<ActionResult<IEnumerable<DtoLoanResponse>>> GetAllAsync();
    }
}
