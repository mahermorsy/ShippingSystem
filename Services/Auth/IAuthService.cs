using Services.APi;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.Dtos;

namespace Services.Auth
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> RegisterAsync(DTOUser model);
        Task<ApiResponse<DTOAuthResponse>> LoginAsync(DTOUser model);
        Task<ApiResponse<DTOAuthResponse>> RefreshTokenAsync(string refreshToken);
    }
}
