using kitapsin.Server.Dto;

namespace kitapsin.Server.Controllers
{
    public interface ICategoryController
    {
        Task<IEnumerable<DtoCategoryResponse>> GetAllAsync();
        Task<DtoCategoryResponse?> GetByIdAsync(int id);
        Task<IEnumerable<DtoCategoryResponse>> SearchByNameAsync(string title);
        Task<DtoCategoryResponse> CreateAsync(DtoCategoryCreate dto);
        Task<bool> UpdateAsync(int id, DtoCategoryUpdate dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<DtoBookResponse>> GetBooksByCategoryAsync(int authorId);
    }
}
