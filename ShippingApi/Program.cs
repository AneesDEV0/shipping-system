using BusinessLayer.Mapping;
using DataAccess.Context;
using DataAccess.UsersModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ShippingApi.Services;
using WebApi.Services;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowFrontend", policy =>
//    { // 7227
//        policy.WithOrigins("https://localhost:7048") // ?? Your MVC project URL
//              .AllowAnyMethod()
//              .AllowAnyHeader()
//                    .WithHeaders("Authorization", "Content-Type") // هنا السماح بهيدر Authorization
//              .AllowCredentials(); // ?? Required for cookies (refresh token)
//    });
//});
//builder.Services.AddControllers()
//              .AddJsonOptions(options =>
//              {
//                  options.JsonSerializerOptions.PropertyNamingPolicy = null; // ?? مهم جدًا
//              });// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//RegisterServciesHelper.RegisterServices(builder);


//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseCors("AllowFrontend");

//app.UseAuthentication(); 
//app.UseAuthorization();

//app.MapControllers();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//    var dbContext = services.GetRequiredService<ShippingContext>();

//    // Apply migrations
//    await dbContext.Database.MigrateAsync();

//    // Seed data
//    await ContextConfig.SeedDataAsync(dbContext, userManager, roleManager);
//}


//app.Run();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:7048") // 👈 Your MVC project URL
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); 
        policy.WithOrigins("https://localhost:7191")
               .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // ?? مهم جدًا
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

RegisterServciesHelper.RegisterServices(builder);

//builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

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