using BlazorApp.Contracts;
using BlazorApp.Helpers.Utilites;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorApp.Services.Auth
{
    public class CustomAuthStateProvidder : AuthenticationStateProvider
    {
        private readonly ISyncLocalStorageService _localStorage;

        public CustomAuthStateProvidder(ISyncLocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = _localStorage.GetItem<string>("AccessToken");
            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // استخراج الـ claims من التوكن
            var claims = JwtParser.ParseClaimsFromJwt(token);
           
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        public void NotifyUserAuthentication(string token)
        {
            var claims = JwtParser.ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            _localStorage.RemoveItem("AccessToken");
            _localStorage.RemoveItem("RefreshToken");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }
    }
}
