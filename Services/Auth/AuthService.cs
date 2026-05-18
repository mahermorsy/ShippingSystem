using BusinessLayer.Dtos;
using UI.UI_Services.APi
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IApiClient _apiClient;

        public AuthService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public Task<ApiResponse<object>> RegisterAsync(DTOUser model)
        {
            return _apiClient.PostAsync<DTOUser, object>("api/Auth/register", model);
        }

        public Task<ApiResponse<DTOAuthResponse>> LoginAsync(DTOUser model)
        {
            return _apiClient.PostAsync<DTOUser, DTOAuthResponse>("api/Auth/login", model);
        }

        public Task<ApiResponse<DTOAuthResponse>> RefreshTokenAsync(string refreshToken)
        {
            var request = new DTORefreshRequest
            {
                RefreshToken = refreshToken
            };

            return _apiClient.PostAsync<DTORefreshRequest, DTOAuthResponse>("api/Auth/refresh-token", request);
        }
    }
}

