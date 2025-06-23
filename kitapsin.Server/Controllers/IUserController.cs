using kitapsin.Server.Dto;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{
    public interface IUserController
    {
        Task<ActionResult<IEnumerable<DtoUserResponse>>> GetAllAsync();
        Task<ActionResult<DtoUserResponse?>> GetByIdAsync(int id);
        Task<ActionResult<DtoUserResponse?>> GetByCardNumberAsync(string cardNumber);
        Task<ActionResult<IEnumerable<DtoUserResponse>>> SearchByNameAsync(string name);
        Task<ActionResult<DtoUserResponse>> CreateAsync(DtoUserCreate dto);
        Task<ActionResult> UpdateAsync(int id, DtoUserUpdate dto);
        Task<ActionResult> DeleteAsync(int id);

        Task<ActionResult<IEnumerable<DtoLoanResponse>>> GetLoansAsync(int userId);
        Task<ActionResult<IEnumerable<DtoPenaltyResponse>>> GetPenaltiesAsync(int userId);
        Task<ActionResult<IEnumerable<DtoBookResponse>>> GetBorrowedBooksAsync(int userId);
    }
}
