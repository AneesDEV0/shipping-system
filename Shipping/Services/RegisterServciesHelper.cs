using BusinessLayer.Contracts;
using BusinessLayer.Mapping;
using BusinessLayer.Services;
using BusinessLayer.Services.Shippment;
using DataAccess.Context;
using DataAccess.Contracts;
using DataAccess.Repositories;
using DataAccess.UsersModel;
using Domains;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net.Http.Headers;

namespace ShippingApp.Services
{
    public static class RegisterServciesHelper
    {
        public static void RegisterServices(WebApplicationBuilder builder)
        {

            builder.Services.AddHttpClient("ApiClient", client =>
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/access-denied";
                    options.SlidingExpiration = true;
                    options.Cookie.IsEssential = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(7); 
                });
            // Add services to the container
            builder.Services.AddDbContext<ShippingContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ShippingContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthorization();

            // Configure Serilog
            Serilog.Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                    tableName: "Log",
                    autoCreateSqlTable: true)
                .CreateLogger();
            builder.Host.UseSerilog();

            // Configure DI Services
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            builder.Services.AddScoped<GenericApiClient>();
            builder.Services.AddScoped(typeof(ITableRepository<>), typeof(TableRepository<>));
            builder.Services.AddScoped(typeof(IViewRepository<>), typeof(ViewRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IShippingTypeService, ShippingTypeService>();
            builder.Services.AddScoped<ICityService, CityService>();
            //builder.Services.AddScoped<ICountryService, CountryService>();
            builder.Services.AddScoped<IRefreshTokens, RefreshTokenService>();
            builder.Services.AddScoped<IRefreshTokenRetriver, RefreshTokenRetriverService>();

            builder.Services.AddScoped<IUserSender, UserSenderService>();
            builder.Services.AddScoped<IUserRecever, UserReceverService>();

            builder.Services.AddScoped<IShipment, ShippmentService>();
            builder.Services.AddScoped<ITrackingNumberCreator, TrackingNumberCreatorService>();
            builder.Services.AddScoped<IRateCalculator, RateCalculatorService>();

            builder.Services.AddScoped<IShipmentStatus, ShipmentStatusService>();

            builder.Services.AddScoped<IShipingPackgingService, ShipingPackgingService>();


        }
    }
}
