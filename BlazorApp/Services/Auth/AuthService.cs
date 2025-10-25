using BlazorApp.Contracts;
using Blazored.LocalStorage;
using Shared.Common;
using Shared.Dtos;
using System.Net.Http.Json;

namespace BlazorApp.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly ISyncLocalStorageService _localStorage;

        public AuthService(HttpClient http, ISyncLocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;

            var accessToken = _localStorage.GetItem<string>("AccessToken");
            if (!string.IsNullOrEmpty(accessToken))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

       
        public async Task<ApiResponse<AuthResponseDto>> login(LoginDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", dto);

            if (!response.IsSuccessStatusCode)
            {
                return ApiResponse<AuthResponseDto>.FailResponse("Login failed due to server error.");
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDto>>();
            if (result?.Data == null)
            {
                return ApiResponse<AuthResponseDto>.FailResponse(result?.Message ?? "Login failed.");
            }
            
            // حفظ التوكنات
            _localStorage.SetItem("AccessToken", result.Data.AccessToken);
            _localStorage.SetItem("RefreshToken", result.Data.RefreshToken);

            // إعداد Authorization header
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Data.AccessToken);

            // إعادة الـ ApiResponse كما هو
            return result;
        }

        public async Task<bool> Register(UserDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", dto);
            if (!response.IsSuccessStatusCode)
            {
                //return ApiResponse<AuthResponseDto>.FailResponse("Register failed due to server error.");
                return false;
            }
            return true;
          
        }

    }
}



/*
 =================================== TASKS ============================
 
 change hi name text color and focous on claims
mange url on buttons
add register page
 add logout
 
 
 */