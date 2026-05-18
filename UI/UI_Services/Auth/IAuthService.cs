using BusinessLayer.Dtos;
using Config;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.UI_Services.APi;

namespace WebApi.UI_Services.Auth
{
    public interface IAuthService
    {
        Task<ApiResponse<RegisterResponse>> RegisterAsync(DTOUser model);
        Task<ApiResponse<DTOAuthResponse>> LoginAsync(DTOLoginUser model);
        Task<ApiResponse<DTOAuthResponse>> RefreshTokenAsync(string refreshToken);
        Task<ApiResponse<DTOAuthResponse>> RefreshAccessTokenAsync(string refreshToken);
    }
}
