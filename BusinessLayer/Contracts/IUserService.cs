using Shared.Dtos;

namespace BusinessLayer.Contracts
{
    public interface IUserService
    {
        Task<UserResultDto> RegisterAsync(UserDto registerDto);
        Task<UserResultDto> LoginAsync(LoginDto loginDto);
        Task LogoutAsync();
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Guid GetLoggedInUser();

    }
}
