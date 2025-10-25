using DataAccess.UsersModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Services;
using BusinessLayer.Contracts;
using Shared.Dtos;
using System.Linq;
using Shared.Dtos;
using Shared.Common;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IRefreshTokens _refreshTokenService;
        private readonly IRefreshTokenRetriver _refreshTokenRetriver;

        public AuthController(
            TokenService tokenService,
            IUserService userService,
            IRefreshTokens refreshTokenService,
            IRefreshTokenRetriver refreshTokenRetriver)
        {
            _tokenService = tokenService;
            _userService = userService;
            _refreshTokenService = refreshTokenService;
            _refreshTokenRetriver = refreshTokenRetriver;
        }

        // ===================== Register =====================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto request)
        {
            var result = await _userService.RegisterAsync(request);
            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("User registered successfully");
        }

        // ===================== Login =====================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var userResult = await _userService.LoginAsync(request);
            if (!userResult.Success)
                return Unauthorized("Invalid credentials");

            var userData = await GetClaims(request.Email);
            var claims = userData.Item1;
            var user = userData.Item2;

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Store refresh token in database
            var storedToken = new RefreshTokenDto
            {
                Token = refreshToken,
                UserId = user.Id.ToString(),
                Expires = DateTime.UtcNow.AddDays(7),
                CurrentState = 1
            };
           await _refreshTokenService.Refresh(storedToken);

            Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None, // مهم لو front و api على دومينات مختلفة
                Expires = storedToken.Expires
            });
            UserDto userDto = new UserDto() { Id = user.Id, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, Role = user.Role, Phone = user.Phone };
            ApiResponse<AuthResponseDto> res = ApiResponse<AuthResponseDto>.SuccessResponse(new AuthResponseDto() 
            { User = userDto ,AccessToken = accessToken , RefreshToken =refreshToken},
            "User retrieved successfully");
            return Ok(res);
            //return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
        }

        // ===================== Refresh Access Token =====================
        //[HttpPost("RefreshAccessToken")]
        //public async Task<IActionResult> RefreshAccessToken()
        //{
        //    if (!Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
        //        return Unauthorized("No refresh token found");

        //    var storedToken = _refreshTokenRetriver.GetByToken(refreshToken);
        //    if (storedToken == null || storedToken.CurrentState == 2 || storedToken.Expires < DateTime.UtcNow)
        //        return Unauthorized("Invalid or expired refresh token");

        //    var claims = await GetClaimsById(storedToken.UserId);
        //    var newAccessToken = _tokenService.GenerateAccessToken(claims);

        //    Response.Cookies.Append("AccessToken", newAccessToken, new CookieOptions
        //    {
        //        HttpOnly = false,
        //        Secure = true,
        //        Expires = DateTime.UtcNow.AddMinutes(15)
        //    });

        //    return Ok(new { AccessToken = newAccessToken });
        //}
        [HttpPost("RefreshAccessToken")]
        public async Task<IActionResult> RefreshAccessToken([FromBody] string refreshToken)
        {
            var storedToken = _refreshTokenRetriver.GetByToken(refreshToken);
            if (storedToken == null || storedToken.CurrentState == 2 || storedToken.Expires < DateTime.UtcNow)
                return Unauthorized("Invalid or expired refresh token");

            var claims = await GetClaimsById(storedToken.UserId);
            var newAccessToken = _tokenService.GenerateAccessToken(claims);

            return Ok(new { AccessToken = newAccessToken });
        }


        // ===================== Refresh Refresh Token =====================
        //[HttpPost("refresh")]
        //public async Task<IActionResult> RefreshRefreshToken()
        //{
        //    if (!Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
        //        return Unauthorized("No refresh token found");

        //    var storedToken = _refreshTokenRetriver.GetByToken(refreshToken);
        //    if (storedToken == null || storedToken.CurrentState == 2 || storedToken.Expires < DateTime.UtcNow)
        //        return Unauthorized("Invalid or expired refresh token");

        //    var newRefreshToken = _tokenService.GenerateRefreshToken();
        //    var newRefreshDto = new RefreshTokenDto
        //    {
        //        Token = newRefreshToken,
        //        UserId = storedToken.UserId,
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        CurrentState = 1
        //    };
        //    _refreshTokenService.Refresh(newRefreshDto);

        //    Response.Cookies.Append("RefreshToken", newRefreshToken, new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,
        //        SameSite = SameSiteMode.None,
        //        Expires = newRefreshDto.Expires
        //    });

        //    return Ok(new { RefreshToken = newRefreshToken });
        //}
        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshRefreshToken([FromBody] string refreshToken)
        {
            var storedToken = _refreshTokenRetriver.GetByToken(refreshToken);
            if (storedToken == null || storedToken.CurrentState == 2 || storedToken.Expires < DateTime.UtcNow)
                return Unauthorized("Invalid or expired refresh token");

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var newRefreshDto = new RefreshTokenDto
            {
                Token = newRefreshToken,
                UserId = storedToken.UserId,
                Expires = DateTime.UtcNow.AddDays(7),
                CurrentState = 1
            };

            _refreshTokenService.Refresh(newRefreshDto);

            return Ok(new { RefreshToken = newRefreshToken });
        }


        // ===================== Helpers =====================
        private async Task<(Claim[], UserDto)> GetClaims(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            var roles = new[] { user.Role ?? "User" };

            var claims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Email));

            return (claims.ToArray(), user);
        }

        private async Task<Claim[]> GetClaimsById(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            var roles = new[] { user.Role ?? "User" };

            var claims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Email));

            return claims.ToArray();
        }
    }
}
