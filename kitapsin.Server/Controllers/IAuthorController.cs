using kitapsin.Server.Dto;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{
    public interface IAuthorController
    {
        Task<ActionResult<IEnumerable<DtoAuthorResponse>>> GetAllAsync();
        Task<ActionResult<DtoAuthorResponse?>> GetByIdAsync(int id);
        Task<ActionResult<IEnumerable<DtoAuthorResponse>>> SearchByTitleAsync(string title);
        Task<ActionResult<DtoAuthorResponse>> CreateAsync(DtoAuthorCreate dto);
        Task<ActionResult> UpdateAsync(int id, DtoAuthorUpdate dto);
        Task<ActionResult> DeleteAsync(int id);
        Task<ActionResult<IEnumerable<DtoBookResponse>>> GetBooksByAuthorIdAsync(int authorId);
    }
}
