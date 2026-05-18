using BusinessLayer.Dtos;
using Config;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.UI_Services.APi;

namespace WebApi.UI_Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IApiClient _apiClient;
        public AuthService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public Task<ApiResponse<RegisterResponse>> RegisterAsync(DTOUser model)
        {
            return _apiClient.PostAsync<DTOUser, RegisterResponse>("api/Auth/register", model);
        }
        public Task<ApiResponse<DTOAuthResponse>> LoginAsync(DTOLoginUser model)
        {
            return _apiClient.PostAsync<DTOLoginUser, DTOAuthResponse>("api/Auth/login", model);
        }
        public Task<ApiResponse<DTOAuthResponse>> RefreshTokenAsync(string refreshToken)
        {
            var request = new DTORefreshRequest
            {
                RefreshToken = refreshToken
            };

            return _apiClient.PostAsync<DTORefreshRequest, DTOAuthResponse>("api/Auth/refresh-token", request);
        }
        public Task<ApiResponse<DTOAuthResponse>> RefreshAccessTokenAsync(string refreshToken)
        {
            var request = new DTORefreshRequest
            {
                RefreshToken = refreshToken
            };

            return _apiClient.PostAsync<DTORefreshRequest, DTOAuthResponse>("api/Auth/refresh-access-token", request);
        }
    }
}

