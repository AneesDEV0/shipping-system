using BlazorApp.Contracts;
using BlazorApp.Contracts.@base;
using BlazorApp.Helpers.Utilites;
using BlazorApp.Services;
using BlazorApp.Services.Auth;
using BlazorApp.Services.@base;
using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace BlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddBlazoredToast();

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(apiBaseUrl!)
            });

            builder.Services.AddHttpClient("ServerAPI", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl!);
            })
.AddHttpMessageHandler<RefreshTokenHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerAPI"));
            builder.Services.AddScoped<RefreshTokenHandler>();


            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvidder>();
            builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            builder.Services.AddScoped<ICountriesService, CountriesService>();
            builder.Services.AddScoped<ICitiesService, CitiesService>();
            builder.Services.AddScoped<ProdectService>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<UserService>();  


            await builder.Build().RunAsync();
        }
    }
}
