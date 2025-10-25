using Shared.Dtos;
using BusinessLayer.Contracts;
using DataAccess.UsersModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                           IHttpContextAccessor accessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = accessor;
        }

        public async Task<UserResultDto> RegisterAsync(UserDto registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
                return new UserResultDto { Success = false, Errors = new[] { "Passwords do not match." } };

            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Phone = registerDto.Phone
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            var roleName = string.IsNullOrEmpty(registerDto.Role) ? "User" : registerDto.Role;
            var roleResult = await _userManager.AddToRoleAsync(user, roleName);

            if (!roleResult.Succeeded)
                return new UserResultDto
                {
                    Success = false,
                    Errors = roleResult.Errors?.Select(e => e.Description)
                };

            return new UserResultDto
            {
                Success = result.Succeeded,
                Errors = result.Errors?.Select(e => e.Description)
            };
        }

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return new UserResultDto { Success = false, Errors = new[] { "User not found." } };

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return new UserResultDto { Success = false, Errors = new[] { "Invalid credentials." } };

            // جمع الـ claims مباشرة
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Email)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            return new UserResultDto
            {
                Success = true,
                Claims = claims.ToArray(),
                User = new UserDto
                {
                    Id = Guid.Parse(user.Id),
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    Role = roles.FirstOrDefault()

                }
            };
        }

        public async Task LogoutAsync() => await _signInManager.SignOutAsync();

        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            return new UserDto
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Role = roles.FirstOrDefault()
            };
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            return new UserDto
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Role = roles.FirstOrDefault()
            };
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return _userManager.Users.Select(u => new UserDto
            {
                Id = Guid.Parse(u.Id),
                Email = u.Email
            });
        }

        public Guid GetLoggedInUser()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
        }
    }
}
