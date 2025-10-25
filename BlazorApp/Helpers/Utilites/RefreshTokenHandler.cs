using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BlazorApp.Helpers.Utilites
{
    public class RefreshTokenHandler : DelegatingHandler
    {
        private readonly ISyncLocalStorageService _localStorage;
        private readonly IHttpClientFactory _httpFactory;
        private readonly NavigationManager navManager;
        private readonly IToastService toastService;
        private static SemaphoreSlim _refreshLock = new SemaphoreSlim(1, 1);

        public RefreshTokenHandler(
            ISyncLocalStorageService localStorage,
            IHttpClientFactory httpFactory,
            NavigationManager navManager,
            IToastService toastService)
        {
            _localStorage = localStorage;
            _httpFactory = httpFactory;
            this.navManager = navManager;
            this.toastService = toastService;

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var accessToken = _localStorage.GetItem<string>("AccessToken");
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _refreshLock.WaitAsync();
                try
                {
                    var newAccessToken = _localStorage.GetItem<string>("AccessToken");

                    if (newAccessToken == accessToken)
                    {
                        var refreshToken = _localStorage.GetItem<string>("RefreshToken");
                        if (!string.IsNullOrEmpty(refreshToken))
                        {
                            var refreshClient = _httpFactory.CreateClient("ServerAPI");
                            var refreshResponse = await refreshClient.PostAsJsonAsync("api/auth/RefreshAccessToken", refreshToken);

                            if (refreshResponse.IsSuccessStatusCode)
                            {
                                var result = await refreshResponse.Content
                                    .ReadFromJsonAsync<Dictionary<string, string>>();

                                if (result != null && result.TryGetValue("AccessToken", out newAccessToken))
                                {
                                    _localStorage.SetItem("AccessToken", newAccessToken);
                                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);
                                    response = await base.SendAsync(request, cancellationToken);
                                }
                            }
                            else
                            {
                                await HandleExpiredRefreshToken();
                            }
                        }
                        else
                        {
                            // Refresh Token غير موجود → تعامل كجلسة منتهية
                            await HandleExpiredRefreshToken();
                        }
                    }
                }
                finally
                {
                    _refreshLock.Release();
                }
            }

            return response;
        }

        private async Task HandleExpiredRefreshToken()
        {
            _localStorage.RemoveItem("AccessToken");
            _localStorage.RemoveItem("RefreshToken");

            // عرض Toast قبل التوجيه
            toastService.ShowWarning("جلسة المستخدم انتهت، الرجاء تسجيل الدخول مجددًا.");

            // إعادة التوجيه لشاشة تسجيل الدخول
            navManager.NavigateTo("/login");
            await Task.CompletedTask;
        }
    }
}
