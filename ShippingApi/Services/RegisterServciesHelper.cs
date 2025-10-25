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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Shared.Dtos;
using ShippingApi.Services;
using System.Text;
using WebApi.Services;
using Log = Serilog.Log;

namespace WebApi.Services
{
    public static class RegisterServciesHelper
    {
        public static void RegisterServices(WebApplicationBuilder builder)
        {

            ////"ThisIsAReallyLongSecretKeyForSigningTokens_ChangeMe"
            ////"ThisIsAReallyLongSecretKeyForSigningTokens_ChangeMe"
            //var jwtSection = builder.Configuration.GetSection("Jwt");
            //var key = jwtSection["Key"];
            //var issuer = jwtSection["Issuer"];
            //var audience = jwtSection["Audience"];
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.RequireHttpsMetadata = true; // في dev = false حسب الحاجة
            //    //options.SaveToken = true;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = false,
            //        //ValidIssuer = issuer,

            //        ValidateAudience = false,
            //        //ValidAudience = audience,

            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            //        //ClockSkew = TimeSpan.FromSeconds(30)
            //    };
            //    // 👇 منع إعادة التوجيه لصفحة Login في الـ API
            //});
            //// Add services to the container
            //builder.Services.AddDbContext<ShippingContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            //{
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.User.RequireUniqueEmail = true;
            //})
            //.AddEntityFrameworkStores<ShippingContext>()
            //.AddDefaultTokenProviders();
            //builder.Services.AddAuthorization();
            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    options.Events.OnRedirectToLogin = context =>
            //    {
            //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //        return Task.CompletedTask;
            //    };
            //    options.Events.OnRedirectToAccessDenied = context =>
            //    {
            //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //        return Task.CompletedTask;
            //    };
            //});
            //// Configure Serilog
            //Serilog.Log.Logger = new LoggerConfiguration()
            //    .WriteTo.Console()
            //    .WriteTo.MSSqlServer(
            //        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
            //        tableName: "Log",
            //        autoCreateSqlTable: true)
            //    .CreateLogger();
            //builder.Host.UseSerilog();

            //var jwtSection = builder.Configuration.GetSection("Jwt");
            //var keysx= jwtSection["Key"];
            //var issuer = jwtSection["Issuer"];
            //var audience = jwtSection["Audience"];
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.RequireHttpsMetadata = true; // في dev = false حسب الحاجة
            //    options.SaveToken = true;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidIssuer = issuer,

            //        ValidateAudience = true,
            //        ValidAudience = audience,

            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keysx)),

            //        ValidateLifetime = true,
            //        ClockSkew = TimeSpan.FromSeconds(30)
            //    };
            //});

            builder.Services.AddDbContext<ShippingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ShippingContext>()
    .AddDefaultTokenProviders();

            var jwtSecretKey = builder.Configuration.GetValue<string>("JwtSettings:SecretKey");
            byte[] key = Encoding.ASCII.GetBytes(jwtSecretKey);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // Configure Serilog for logging
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                    tableName: "Log",
                    autoCreateSqlTable: true)
                .CreateLogger();
            builder.Host.UseSerilog();

            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            builder.Services.AddScoped(typeof(ITableRepository<>), typeof(TableRepository<>));
            builder.Services.AddScoped(typeof(IViewRepository<>), typeof(ViewRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IShippingTypeService, ShippingTypeService>();
            builder.Services.AddScoped<ICityService, CityService>();
            builder.Services.AddScoped<ISenderService, CountryService>();
            builder.Services.AddSingleton<TokenService>();
            builder.Services.AddScoped<IRefreshTokens, RefreshTokenService>();
            builder.Services.AddScoped<IRefreshTokenRetriver, RefreshTokenRetriverService>();

            builder.Services.AddScoped<IUserSender, UserSenderService>();
            builder.Services.AddScoped<IUserRecever, UserReceverService>();

            builder.Services.AddScoped<IShipment, ShippmentService>();
            builder.Services.AddScoped<ITrackingNumberCreator, TrackingNumberCreatorService>();
            builder.Services.AddScoped<IRateCalculator, RateCalculatorService>();
            builder.Services.AddScoped<IShipmentStatus, ShipmentStatusService>();

            builder.Services.AddScoped<IBaseService<TbCountry, CountryDto>, CountryService>();


            builder.Services.AddScoped<IShipingPackgingService, ShipingPackgingService>();


        }
    }
}
