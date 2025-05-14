using AssessmentProject.Repository.Interfaces;
using AssessmentProject.Repository.Models;
using AssessmentProject.Repository.Repositories;
using AssessmentProject.Service.Interfaces;
using AssessmentProject.Service.Services;
using AssessmentProject.Service.Utilities;
using AssessmentProject.Service.Utilities.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database Context
string? conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AssessmentProjectContext>(options => options.UseNpgsql(conn));

// Dependency Injection
// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IJwtUtility, JwtUtility>();

builder.Services.AddAuthorization();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStatusCodePagesWithReExecute("/Error/NotFound");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");
app.Run();
