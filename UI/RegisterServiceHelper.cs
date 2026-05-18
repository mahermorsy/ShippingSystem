 using AutoMapper;
using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using BusinessLayer.Helpers;
using BusinessLayer.Mapping;
using BusinessLayer.Services;
using BusinessLayer.Services.Payment;
using BusinessLayer.Services.ShipmentFile;
using BusinessLayer.Services.ShipmentFile.ManageStatus;
using Config;
using DataAccessLayer.Data;
using DataAccessLayer.Identity;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Domains.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Net.NetworkInformation;
using System.Text;
using UI.Services;
using WebApi.UI_Services;
using WebApi.UI_Services.APi;
using WebApi.UI_Services.Auth;
using WebApi.UI_Services.ShipmentServices;
namespace WebApi
{
    public class RegisterServiceHelper
    {
        public static void RegisterServices(WebApplicationBuilder builder)
        {

            builder.Services.Configure<AuthApiSettings>(builder.Configuration.GetSection("AuthApi"));


            #region Services builder Configuration
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddDbContext<ShippingContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("Shipping")));

            builder.Services.AddHttpClient<IApiClient, ApiClient>((serviceProvider, client) =>
            {
                var options = serviceProvider
                    .GetRequiredService<Microsoft.Extensions.Options.IOptions<AuthApiSettings>>()
                    .Value;

                client.BaseAddress = new Uri(options.BaseUrl);
            });

            //builder.Services.AddScoped<IGenericRepository<TbShippingType>, GenericRepository<TbShippingType>>();
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IVwRepository<>), typeof(VwRepository<>));
            builder.Services.AddScoped<IVwCityService, VwCityService>();
            builder.Services.AddScoped<ICountries, CountryService>();
            builder.Services.AddScoped<IRefreshToken, RefreshTokenService>();
            builder.Services.AddScoped<ICarriers, CarrierService>();
            builder.Services.AddScoped<ICities, CityService>();
            builder.Services.AddScoped<IPaymentMethods, PaymentMethodService>();
            builder.Services.AddScoped<IShipmentApiService, ShipmentApiService>();
            builder.Services.AddScoped<ISettings, SettingService>();
            builder.Services.AddScoped<IShippingTypes, ShippingTypeService  >();
            builder.Services.AddScoped<IShipmentCommand,ShipmentCommandService>();
            builder.Services.AddScoped<IShipmentQuery, ShipmentQueryService>();
            builder.Services.AddScoped<IShipmentStatus, ShipmentStatusService>();
            builder.Services.AddScoped<ISubscriptionPackage, SubscriptionPackageService>();
            builder.Services.AddScoped<IUserReceiver, UserReceiverService>();
            builder.Services.AddScoped<IUserSender, UserSenderService>();
            builder.Services.AddScoped<IUserSubscription, UserSubscriptionService>();
            builder.Services.AddScoped<IRateCalculator,RateCalculator>();
            builder.Services.AddScoped<ITrackingNumber, TrackingNumber>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped(typeof(IVwRepository<>), typeof(VwRepository<>));
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IShippingPackaging, ShippingPackagingService>();
            builder.Services.AddScoped<IShipmentStatusHandlerFactory, ShipmentStatusHandlerFactory>();
            builder.Services.AddScoped<ApproveService>();
            builder.Services.AddScoped<ReadyForShipmentService>();
            builder.Services.AddScoped<ShippedService>();
            builder.Services.AddScoped<DeliveredService>();
            builder.Services.AddScoped<ReturnedService>();
            builder.Services.AddHttpClient<PayPalGateway>();
            builder.Services.AddHttpClient<PaymobGetAway>();
            builder.Services.AddScoped<PaymentFactory>();

            //builder.Services.AddSingleton<TokenService>(); 
            builder.Services.AddHttpContextAccessor();
            #endregion
            #region Jwt Configuration
            /*var jwtSettings = builder.Configuration.GetSection("Jwt");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                };
            }); */

            var jwtSettings = builder.Configuration.GetSection("Jwt");

            // ✅ الشكل النهائي الصحيح
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.SlidingExpiration = true;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                    };
                });
            #endregion

            #region Auto Mapper
            builder.Services.AddAutoMapper(cfg => { }, typeof(AutoMappingProfile).Assembly);
            #endregion

            #region Serilog Configuration
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                if (context.HostingEnvironment.IsDevelopment())
                {
                    configuration.WriteTo.Console();
                    return;
                }

                configuration
                    .WriteTo.Console()
                    .WriteTo.MSSqlServer(
                        connectionString: context.Configuration.GetConnectionString("Shipping"),
                        sinkOptions: new MSSqlServerSinkOptions
                        {
                            TableName = "Log",
                            AutoCreateSqlTable = true
                        });
            });
            #endregion

            #region Identity And Cookies Configuration

            //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
            // Option =>
            // {
            //     Option.LoginPath = "/Login";
            //     Option.AccessDeniedPath = "/AccessDenied";
            // });

            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = "/Login";
            //    options.AccessDeniedPath = "/AccessDenied";
            //});
            // 5.نظام الهوية

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true; // ✅ إضافة

                // Lockout settings (✅ حماية من محاولات الدخول المتكررة)
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true; // ✅ البريد فريد
            })
            .AddEntityFrameworkStores<ShippingContext>() // ربط Identity بقاعدة البيانات
            .AddDefaultTokenProviders(); // توليد رموز التحقق (مثل إعادة تعيين كلمة المرور)

            // ✅ إعدادات الـ Cooki
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(7);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthorization();
            #endregion

        }
    }
}
