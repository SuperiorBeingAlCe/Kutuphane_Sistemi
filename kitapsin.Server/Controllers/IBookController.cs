using kitapsin.Server.Dto;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{
    public interface IBookController
    {
        Task<ActionResult<IEnumerable<DtoBookResponse>>> GetAll();
        Task<ActionResult<DtoBookResponse>> GetById(int id);
        Task<ActionResult<IEnumerable<DtoBookResponse>>> SearchByTitle(string title);
        Task<ActionResult<DtoBookResponse>> Create(DtoBookCreate dto);
        Task<IActionResult> Update(int id, DtoBookUpdate dto);
        Task<IActionResult> Delete(int id);


    }
}
