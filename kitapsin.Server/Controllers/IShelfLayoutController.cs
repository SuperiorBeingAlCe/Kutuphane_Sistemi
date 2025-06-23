using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{
    public interface IShelfLayoutController
    {
        Task<IActionResult> GetShelfLayoutsAsync();
        Task<IActionResult> GetShelfLayoutByIdAsync(int id);
        Task<IActionResult> DeleteShelfLayoutAsync(int id);
    }
}
