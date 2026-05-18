using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.UI_Services.APi
{
    public interface IApiClient
    {
        Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url, bool withToken = false);
        Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest request, bool withToken = false);
        Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest request, bool withToken = false);
        Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string url, bool withToken = false);
    }
}
