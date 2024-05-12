using Binel.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BinelProjectContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlcon"));

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

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
