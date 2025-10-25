using BusinessLayer.Contracts;
using Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingApp.Services;
using ShippingApp.Models;
using Shared.Common;

namespace Ui.Controllers
{
    public class AccountController : Controller
    {
        IUserService _userService;
        private readonly GenericApiClient _apiClient;
        public AccountController(IUserService userService, GenericApiClient apiClient)
        {
            _userService = userService;
            _apiClient = apiClient;
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var result = await _userService.LoginAsync(user);
            if (result.Success)
            {
                // Call the login API using the generic client
                ApiResponse<AuthResponseDto> requset = await _apiClient.PostAsync<ApiResponse<AuthResponseDto>>("api/auth/login", user); 
                LoginApiModel apiResult = new LoginApiModel() { AccessToken = requset.Data.AccessToken, RefreshToken = requset.Data.RefreshToken };

                if (apiResult == null)
                {
                    ModelState.AddModelError(string.Empty, "API error: Unable to process login.");
                    return View(user);
                }

                var accessToken = apiResult?.AccessToken.ToString();

                if (string.IsNullOrEmpty(accessToken))
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(user);
                }
                // Store the access token in the cookie (for subsequent requests)
                Response.Cookies.Append("AccessToken", accessToken, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(15)  // Adjust token expiry based on your needs
                });

                var dbUser = await _userService.GetUserByEmailAsync(user.Email);

                if (dbUser.Role.ToLower() == "admin")
                    return RedirectToRoute(new { area = "admin", controller = "Home", action = "Index" });
                else
                    return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
                return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserDto user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var result = await _userService.RegisterAsync(user);
            if (result.Success)
            {
                return RedirectToRoute(new { controller = "Account", action = "Login" });
            }
            else
                return View();
        }

        [Authorize]
        public async Task<IActionResult> SignOut()
        {
           await _userService.LogoutAsync();
            return View("Login");
        }

    }
}
