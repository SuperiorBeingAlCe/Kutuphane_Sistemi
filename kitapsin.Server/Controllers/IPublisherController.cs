using kitapsin.Server.Dto;

namespace kitapsin.Server.Controllers
{
    public interface IPublisherController
    {
        Task<IEnumerable<DtoPublisherResponse>> GetAllAsync();
        Task<DtoPublisherResponse> GetByIdAsync(int id);
        Task<IEnumerable<DtoPublisherResponse>> SearchByNameAsync(string name);
        Task<DtoPublisherResponse> CreateAsync(DtoPublisherCreate dto);
        Task<bool> UpdateAsync(int id, DtoPublisherUpdate dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<DtoBookResponse>> GetBooksByPublisherIdAsync(int publisherId);
    }
}
