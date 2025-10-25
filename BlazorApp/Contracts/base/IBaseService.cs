using Shared.Common;

namespace BlazorApp.Contracts.@base
{
    public interface IBaseService<DTO>
    {
        Task<ApiResponse<List<DTO>>> GetAll();
        Task<ApiResponse<DTO>> GetById(Guid id);
        Task<ApiResponse<DTO>> Add(DTO entity);
        Task<ApiResponse<DTO>> Update(Guid id, DTO entity);
        Task<ApiResponse<bool>> Delete(Guid id);
    }
}
