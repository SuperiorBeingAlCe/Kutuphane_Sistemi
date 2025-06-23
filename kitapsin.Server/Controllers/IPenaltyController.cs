using kitapsin.Server.Dto;
using Microsoft.AspNetCore.Mvc;

namespace kitapsin.Server.Controllers
{
    public interface IPenaltyController
    {
        Task<ActionResult<DtoPenaltyResponse>> AddAsync([FromBody] DtoPenaltyCreate dto);
        Task<ActionResult> PayAndRemoveAsync(int id);
    }
}
