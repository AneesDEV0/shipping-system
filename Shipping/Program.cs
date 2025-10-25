using Microsoft.EntityFrameworkCore;
using Serilog;
using DataAccess.Context;
using Domains;
using DataAccess.Contracts;
using DataAccess.Repositories;
using BusinessLayer.Contracts;
using BusinessLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using BusinessLayer.Mapping;
using Microsoft.AspNetCore.Authentication.Cookies;
using DataAccess.UsersModel;
using Microsoft.AspNetCore.Identity;
using ShippingApp.Services;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddLogging();

//builder.Services.AddScoped<ITableRepository<TbShippingType>, TableRepository<TbShippingType>>();
//builder.Services.AddScoped<ITableRepository<TbCountry>, TableRepository<TbCountry>>();
//builder.Services.AddScoped<ITableRepository<TbCity>, TableRepository<TbCity>>();
////builder.Services.AddScoped(typeof(ITableRepository<>), typeof(TableRepository<>));

////builder.Services.AddScoped<IViewRepository<VwCities>, IViewRepository<VwCities>>();
//builder.Services.AddScoped(typeof(IViewRepository<>), typeof(ViewRepository<>));

// Add DbContext and Identity

builder.Services.AddControllersWithViews();
// Configure Serilog
RegisterServciesHelper.RegisterServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
// add for services
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
// add for routing areas
#region Routing
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}");

    endpoints.MapControllerRoute(
    name: "LandingPages",
    pattern: "{area:exists}/{controller=Home}/{action=Index}");

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
    name: "ali",
    pattern: "ali/{controller=Home}/{action=Index}/{id?}");
}
);
#endregion

// for add Seed Data 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var dbContext = services.GetRequiredService<ShippingContext>();

    // Apply migrations
    await dbContext.Database.MigrateAsync();

    // Seed data
    await ContextConfig.SeedDataAsync(dbContext, userManager, roleManager);
}


app.Run();
