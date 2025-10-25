using BlazorApp.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorApp.Services.Auth
{
    public class UserService
    {
        private readonly AuthenticationStateProvider _authProvider;

        public UserService(AuthenticationStateProvider authProvider)
        {
            _authProvider = authProvider;
        }

        public async Task<string?> GetUserIdAsync()
        {
            var authState = await _authProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated ?? false)
            {
                return  user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    
            }

            return null;
        }
        public async Task<CurrentUser> GetCurrentUserAsync()
        {
            var authState = await _authProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            return new CurrentUser
            {
                IsAuthenticated = user.Identity?.IsAuthenticated ?? false,
                UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = user.FindFirst(ClaimTypes.Name)?.Value,
                Role = user.FindFirst(ClaimTypes.Role)?.Value
            };
        }
    }
}
