using DataAccessLayer.Data;
using DataAccessLayer.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // ✅ صح
using Serilog;
using WebApi;
using WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BusinessLayer.Helpers;

var builder = WebApplication.CreateBuilder(args); 
// Services
builder.Services.AddControllers();

// JSON Serialization Configuration
//builder.Services.AddControllers().AddJsonOptions(
//    Option =>
//    {
//        Option.JsonSerializerOptions.PropertyNamingPolicy = null;   
//    });
#region Cross-Origin Resource Sharing (CORS) Configuration  
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowfromAnyOrigin", policy =>
    {
        policy.WithOrigins("https://localhost:7279", "http://localhost:5081")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
#endregion

#region OpenAPI \ Swagger Configuration    
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Shipping System API",
        Version = "v1",
        Description = "API for Shipping System",
        Contact = new OpenApiContact
        {
            Name = "Maher Al-Mubarak",
            Email = "Maher.Al-Mubarak@Qncept.com"
        }
    });
});

#endregion

RegisterServiceHelper.RegisterServices(builder);

// Serilog
//builder.Host.UseSerilog();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowfromAnyOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSession();



// Migration + Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ShippingContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        await context.Database.MigrateAsync();
        await ContexConfig.SeedDataAsync(context, userManager, roleManager);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Log.Error(ex, "Error during migration or seeding");
    }
}

app.Run();
