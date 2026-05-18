using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using Config;
using DataAccessLayer.Identity;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;
namespace UI.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IHttpContextAccessor _HttpContextAccessor;
       public UserService(SignInManager<ApplicationUser> SinginManager, 
           UserManager<ApplicationUser> UserManager,
           IHttpContextAccessor HttpContextAccesso)
        {
            _signInManager = SinginManager;
            _UserManager = UserManager;
            _HttpContextAccessor = HttpContextAccesso;
        }
        public async Task<RegisterResponse> RegisterAsync(DTOUser UserRegister)
        {
            var errors = new List<string>();

            if (UserRegister.Password != UserRegister.ConfirmationPassword)
                errors.Add("Passwords do not match");

            if (await _UserManager.FindByNameAsync(UserRegister.UserName) != null)
                errors.Add("Username already exists");

            if (!string.IsNullOrWhiteSpace(UserRegister.Email) &&
                await _UserManager.FindByEmailAsync(UserRegister.Email) != null)
                errors.Add("Email already exists");

            if (!string.IsNullOrWhiteSpace(UserRegister.PhoneNumber) &&
                _UserManager.Users.Any(u => u.PhoneNumber == UserRegister.PhoneNumber))
                errors.Add("Phone number already exists");

            if (errors.Any())
            {
                return new RegisterResponse
                {
                    Success = false,
                    Errors = errors
                };
            }

            var user = new ApplicationUser
            {
                UserName = UserRegister.UserName,
                FirstName = UserRegister.FirstName,
                LastName = UserRegister.LastName,
                PhoneNumber = UserRegister.PhoneNumber,
                Email = UserRegister.Email
            };

            var result = await _UserManager.CreateAsync(user, UserRegister.Password);

            if (!result.Succeeded)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "User creation failed",
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            var role = string.IsNullOrWhiteSpace(UserRegister.Role) ? "User" : UserRegister.Role;
            var roleResult = await _UserManager.AddToRoleAsync(user, role);
        
            if (!roleResult.Succeeded)
            {   
                return new RegisterResponse
                {
                    Success = false,
                    Message = "User created but failed to assign role",
                    Errors = roleResult.Errors.Select(e => e.Description)
                };
            }

            return new RegisterResponse
            {
                Success = true,
                Message = "User registered successfully",
                Errors = null
            };
        }
        public async Task<DTOUserResult> LoginAsync(DTOLoginUser UserLogin)
        {
            var SigninResult = await _signInManager.PasswordSignInAsync(UserLogin.UserName, UserLogin.Password, true, false);

            if (!SigninResult.Succeeded)
            {
                return new DTOUserResult
                {   
                    Sucess = false,
                    Errors = new[] { "Username or password not match" }
                };
            }
            return new DTOUserResult
            {
                Sucess = SigninResult.Succeeded,
            };
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<DTOUser> GetUserbyIdAsync(Guid Userid)
        {
            var User = await _UserManager.FindByIdAsync(Userid.ToString());

            if (User == null)
            {
                return null;
            }

            return new DTOUser
            {
                Id = User.Id,
                UserName = User.UserName,
                Email = User.Email,
                FirstName = User.FirstName,
                LastName = User.LastName,
                PhoneNumber = User.PhoneNumber,
                Role = (await _UserManager.GetRolesAsync(User)).FirstOrDefault()    
            };
        }
        public async Task<DTOUser?> GetUserByNameAsync(string username)
        {
            var User = await _UserManager.FindByNameAsync(username);
            if (User == null)
            {
                return null;
            }

            return new DTOUser
            {
                Id = User.Id,
                UserName = User.UserName,
                Email = User.Email,
                FirstName = User.FirstName,
                LastName = User.LastName,
                PhoneNumber = User.PhoneNumber,
                Role = (await _UserManager.GetRolesAsync(User)).FirstOrDefault()
            };

        }
        public async Task<IEnumerable<DTOUser>> GetAllUsersAsync()
        {
            var Users = _UserManager.Users;
            var userList = new List<DTOUser>();
            foreach (var user in Users)
            {
                var role = (await _UserManager.GetRolesAsync(user)).FirstOrDefault();

                userList.Add(new DTOUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Role = role
                });
            }
            return userList;
        }
        public Guid GetLoggedInUser() 
        {
           // var RefreshToken = _HttpContextAccessor.HttpContext?.Request.Cookies["RefreshToken"];
            var LoggedInUser = _HttpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(LoggedInUser))
                throw new UnauthorizedAccessException("No User logged in.");

            return Guid.Parse(LoggedInUser);      
        }
        public async Task<Claim[]> GetClaims(string Username)
        {
            var user = await _UserManager.FindByNameAsync(Username);
            if (user == null)
            {
                return Array.Empty<Claim>();
            }
            var roles = await _UserManager.GetRolesAsync(user);
            var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims.ToArray();
        }

    }
}

