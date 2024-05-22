using Binel.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Duygu Start - Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
    });
// Duygu Finish

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database context configuration
builder.Services.AddDbContext<BinelProjectContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlcon"));
});

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturumun zaman aşımı süresi
    options.Cookie.HttpOnly = true; // Çerezleri sadece HTTP üzerinden erişilebilir yapar
    options.Cookie.IsEssential = true; // Çerezleri temel yapar
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Duygu Start - Session
app.UseSession(); // Session middleware'ini ekleyin
// Duygu Finish

// Route configurations
app.MapControllerRoute(
    name: "Edit",
    pattern: "Users/Edit/{userId}",
    defaults: new { controller = "Users", action = "Edit" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "register",
    pattern: "register",
    defaults: new { controller = "Users", action = "Register" });

app.MapControllerRoute(
    name: "login",
    pattern: "login",
    defaults: new { controller = "Users", action = "Login" });

app.MapControllerRoute(
    name: "feedSearch",
    pattern: "feed/search",
    defaults: new { controller = "Feed", action = "Search" });

app.MapControllerRoute(
    name: "organizationProfile",
    pattern: "{organizationTitle}",
    defaults: new { controller = "Organization", action = "Profile" });

app.MapControllerRoute(
    name: "donatePost",
    pattern: "{organizationTitle}/donatepost",
    defaults: new { controller = "Organization", action = "DonatePost" });

app.MapControllerRoute(
    name: "post",
    pattern: "{organizationTitle}/post",
    defaults: new { controller = "Organization", action = "Post" });

app.Run();
