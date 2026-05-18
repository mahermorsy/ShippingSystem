using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using BusinessLayer.Helpers;
using DataAccessLayer.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Config;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IRefreshToken _refreshTokenService;
        private readonly IUserService _UserService;
        private readonly JwtSettings _jwtSettings;
        public AuthController(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings,  
            ITokenService tokenService,
            IRefreshToken refreshTokenService,
            IUserService userService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _UserService = userService;
            _jwtSettings = jwtSettings.Value;
        }
        [HttpPost("register")] 
        public async Task<ActionResult<RegisterResponse>> Register( DTOUser USerModel)
        {
            var RegResult = await _UserService.RegisterAsync(USerModel);
            if (!RegResult.Success) 
            {
                return BadRequest(
                    new RegisterResponse { Success = false, Errors = RegResult.Errors, Message = RegResult.Message });
            }
            return Ok(new RegisterResponse { Success = true, Message = RegResult.Message });
        }   

        [HttpPost("login")]
        public async Task<ActionResult<DTOAuthResponse>> Login(DTOLoginUser LoginModel)
        {
            var LoginResult = await _UserService.LoginAsync(LoginModel);
     
            if (!LoginResult.Sucess)
                return Unauthorized("Invalid username or password");

            var user = await _UserService.GetUserByNameAsync(LoginModel.UserName);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var accessToken = await _tokenService.CreateAccessTokenAsync(user);
            var refreshToken = _tokenService.CreateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtSettings.DurationInDays);

            var DTORefreashToken = new DTORefreashToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresOn = refreshTokenExpiration
            };

            await _refreshTokenService.RefreshTokenClearAdd(DTORefreashToken);

            return Ok(new DTOAuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = refreshTokenExpiration
            });
        }

        [HttpPost("refresh-access-token")]
        public async Task<ActionResult<DTOAuthResponse>> RefreshAccessToken([FromBody] DTORefreshRequest LoginModel)
        {
            var refreshTokenDto =  _refreshTokenService.GetByToken(LoginModel.RefreshToken);
            if (refreshTokenDto == null || refreshTokenDto.Result.ExpiresOn < DateTime.UtcNow || refreshTokenDto.Result.CurrentState == 1) 
            {
                return BadRequest("Invalid or expired refresh token");
            }

            var user = await _UserService.GetUserbyIdAsync(refreshTokenDto.Result.UserId);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var newAccessToken = await _tokenService.CreateAccessTokenAsync(user);

            return Ok(new DTOAuthResponse
            { 
                AccessToken = newAccessToken,
                RefreshToken = refreshTokenDto.Result.Token,
                RefreshTokenExpiration = refreshTokenDto.Result.ExpiresOn
            });
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<DTOAuthResponse>> RefreshRefreshToken([FromBody] DTORefreshRequest LoginModel) 
        {
            /* if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken )) 
           {
               return BadRequest("Refresh token not found");
           }   
           var refreshTokenDto =  _refreshTokenService.GetByToken(refreshToken);
          */
            var refreshTokenDto = _refreshTokenService.GetByToken(LoginModel.RefreshToken);
            if (refreshTokenDto == null || refreshTokenDto.Result.ExpiresOn < DateTime.UtcNow || refreshTokenDto.Result.CurrentState == 1)
            {
                return BadRequest("Invalid or expired refresh token");
            }
            var user = await _UserService.GetUserbyIdAsync(refreshTokenDto.Result.UserId);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var NewRefreshToken = _tokenService.CreateRefreshToken();   
            var newAccessToken = await _tokenService.CreateAccessTokenAsync(user);

            var DTORefreashToken = new DTORefreashToken
            {
                Token = NewRefreshToken,
                UserId = user.Id,
                ExpiresOn = DateTime.UtcNow.AddDays(_jwtSettings.DurationInDays)
            };

            await _refreshTokenService.RefreshTokenClearAdd(DTORefreashToken);

            return Ok(new DTOAuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = NewRefreshToken,
                RefreshTokenExpiration = DTORefreashToken.ExpiresOn
            });
        }


    }
}