using Shared.Common;
using Shared.Dtos;

namespace BlazorApp.Contracts
{
    public interface IAuthService
    {
        public  Task<ApiResponse<AuthResponseDto>> login(LoginDto dto);
        public Task<bool> Register(UserDto dto);

    }
}
