using Library.Interfaces;
using Library.Models;
using Library.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Interfaces
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserBookRepository, UserBookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
//DB Context
builder.Services.AddDbContext<ApplicationDbContext>(
options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Identity Framework Setup
builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();
//Add this for a failsafe
builder.Services.AddMemoryCache();
builder.Services.AddSession();
//Adding Cookie Auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
var app = builder.Build();

// Configure the HTTP request pipeline. 
if (!app.Environment.IsDevelopment()){
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configure logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole(); // You can add other logging providers as needed
});


// Seed data
using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    SeedData.InitializeAsync(scope.ServiceProvider).Wait();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
