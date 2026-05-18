using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi.UI_Services.APi
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient; 
            _httpContextAccessor = httpContextAccessor; // Inject IHttpContextAccessor to access session for token retrieval
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }
        public async Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url, bool withToken = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            AddBearerToken(request, withToken);

            var response = await _httpClient.SendAsync(request);
            return await HandleResponse<TResponse>(response);
        }

        public async Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest requestBody, bool withToken = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            AddBearerToken(request, withToken);

            var json = JsonSerializer.Serialize(requestBody);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            return await HandleResponse<TResponse>(response);
        }

        public async Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest requestBody, bool withToken = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            AddBearerToken(request, withToken);

            var json = JsonSerializer.Serialize(requestBody);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            return await HandleResponse<TResponse>(response);
        }

        public async Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string url, bool withToken = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            AddBearerToken(request, withToken);

            var response = await _httpClient.SendAsync(request);
            return await HandleResponse<TResponse>(response);
        }

        private void AddBearerToken(HttpRequestMessage request, bool withToken)
        {
            if (!withToken) return;

            var token = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        private async Task<ApiResponse<TResponse>> HandleResponse<TResponse>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            TResponse? data = default;

            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    data = JsonSerializer.Deserialize<TResponse>(content, _jsonOptions);
                }
                catch (JsonException)
                {
                    // الـ response مش JSON — نكمل بدون data
                }
            }

            if (response.IsSuccessStatusCode)
            {
                return ApiResponse<TResponse>.SuccessResponse(
                    data!,
                    "Success",
                    (int)response.StatusCode
                );
            }

            var message = "Request failed";
            var errors = new List<string>();

            if (data != null)
            {
                var messageProperty = typeof(TResponse).GetProperty("Message");
                if (messageProperty?.GetValue(data) is string responseMessage &&
                    !string.IsNullOrWhiteSpace(responseMessage))
                {
                    message = responseMessage;
                }
                var errorProperty = typeof(TResponse).GetProperty("Errors");
                if (errorProperty?.GetValue(data) is List<string> responseErrors)
                {
                    errors = responseErrors;
                }
            }

            if (!errors.Any() && !string.IsNullOrWhiteSpace(content))
            {
                message = content;
            }

            return new ApiResponse<TResponse>
            {
                Success = false,
                Data = data,
                Message = message,
                StatusCode = (int)response.StatusCode,
                Errors = errors
            };
        }
    }
}
