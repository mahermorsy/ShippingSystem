using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi.UI_Services.APi
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; } = default(T?);
        public int StatusCode { get; set; }
        public List<string>? Errors { get; set; } = new List<string>();
        public static ApiResponse<T> SuccessResponse(T data = default(T?), string message = "Success", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message,
                StatusCode = statusCode,
            };
        }   
        public static ApiResponse<T> FailureResponse(T data , string message, int statusCode = 400, List<string> errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Message = message,
                StatusCode = statusCode,
                Errors = errors ?? new List<string>()
            };
        }   
    }
}
