using BusinessLayer.Contracts;  
using BusinessLayer.Services;  
using DataAccessLayer;
using DataAccessLayer.Data;
using DataAccessLayer.Identity;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using WebApi;
using WebApi.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
RegisterServiceHelper.RegisterServices(builder);

#region Cookie ConfigureServices  
/*builder.Services.ConfigureApplicationCookie(options =>
{
    
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;

});*/
#endregion

#region AppSettings And routing
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// Routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
#endregion


#region Seed Data   

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ShippingContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        // Apply Migration
        await context.Database.MigrateAsync();

        // Seed Data
        await ContexConfig.SeedDataAsync(context, userManager, roleManager);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Serilog.Log.Error(ex, "Error during migration or seeding");
    }
#endregion

}

app.Run();
