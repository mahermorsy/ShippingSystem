using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi.UI_Services.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi.UI_Services.APi;

namespace UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _AuthService;
        public AccountController(IUserService userService,
            IAuthService AuthService)
        {
            _userService = userService;
            _AuthService = AuthService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(DTOLoginUser User)
        {
            var SignInresult = await _AuthService.LoginAsync(User);

            if (SignInresult.Success && SignInresult.Data != null)
            {
                HttpContext.Session.SetString("AccessToken", SignInresult.Data.AccessToken);
                HttpContext.Session.SetString("RefreshToken", SignInresult.Data.RefreshToken);
                HttpContext.Session.SetString("RefreshTokenExpiration", SignInresult.Data.RefreshTokenExpiration.ToString());

                /*var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(SignInresult.Data.AccessToken);

                // ReadJwtToken بيرجع الـ claims بأسمائها المختصرة من الـ JWT
                // مثلاً: "role", "nameid", "unique_name", "email"
                // لازم نعمل map لها لأسماء .NET الكاملة عشان IsInRole و SecurityStampValidator يشتغلوا صح
                var mappedClaims = jwtToken.Claims.Select(c =>
                    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.TryGetValue(c.Type, out var mapped)
                        ? new Claim(mapped, c.Value)
                        : c
                ).ToList();

                var identity = new ClaimsIdentity(mappedClaims, IdentityConstants.ApplicationScheme);
                var principal = new ClaimsPrincipal(identity); */

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(SignInresult.Data.AccessToken);

                var claims = jwtToken.Claims.ToList();

                var identity = new ClaimsIdentity(
                    claims,
                    IdentityConstants.ApplicationScheme
                );

                var principal = new ClaimsPrincipal(identity);
                var AuthProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddDays(7),
                    AllowRefresh = true,
                    IsPersistent = true,
                };
                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal, AuthProperties);

                Response.Cookies.Append("AccessToken", SignInresult.Data.AccessToken, new CookieOptions
                {
                    HttpOnly = false, // عشان JS يقدر يقرأه
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
                Response.Cookies.Append("RefreshToken", SignInresult.Data.RefreshToken, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                if (principal.IsInRole("Admin") || principal.IsInRole("Reviewer") ||
                    principal.IsInRole("Operation") || principal.IsInRole("Operation Manager"))
                {
                    return RedirectToRoute(new { area = "admin", controller = "Home", action = "Index" });
                }
                else if (principal.IsInRole("User"))
                {
                    return RedirectToRoute(new { controller = "Home", action = "Index" });
                }
                else
                {
                    // لو مفيش رول معروف
                    return RedirectToAction("AccessDenied", "Account");
                }
            }
            ModelState.AddModelError("", SignInresult.Message);
            return View(User);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(DTOUser user)
        {
            var RegisterResult = await _AuthService.RegisterAsync(user);

            if (RegisterResult.Success)
            {
                return RedirectToAction("Login");
            }
            else
            {
                if (RegisterResult.Data?.Errors != null)
                {
                    foreach (var error in RegisterResult.Data.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if ((RegisterResult.Data?.Errors == null || !RegisterResult.Data.Errors.Any()) &&
                    !string.IsNullOrWhiteSpace(RegisterResult.Message))
                {
                    ModelState.AddModelError("", RegisterResult.Message);

                }

                return View(user);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            HttpContext.Session.Clear();
            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
