using Azure.Core;
using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
namespace BusinessLayer.Services.Payment
{
    public class PayPalGateway : IPayPalGateway
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public PayPalGateway(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }
        public async Task<string>GetPaymentAccessTokenAsync()
        {
            var clientId = _config["PayPal:ClientId"];
            var clientSecret = _config["PayPal:ClientSecret"] ?? _config["PayPal:Secret"];
            var environment = _config["PayPal:Environment"];
            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret) || string.IsNullOrWhiteSpace(environment))
            {
                throw new Exception("PayPal credentials are missing.");
            }

        

            var Url = IsSandbox(environment) ?
                "https://api-m.sandbox.paypal.com/v1/oauth2/token" :
                "https://api-m.paypal.com/v1/oauth2/token"; 

            /*PaypalServerSdkClient client =
               new PaypalServerSdkClient.Builder()
              .Environment(Environment.Sandbox)
              .ClientCredentialsAuth(
                 new ClientCredentialsAuthModel
                    .Builder(clientId, clientSecret)
                    .Build()
            )
            .Build();*/

            var request = new HttpRequestMessage(HttpMethod.Post, Url);

            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", 
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));

            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"PayPal token request failed: {content}");
            }

            var tokenResponse = JsonSerializer.Deserialize<PayPalTokenResponse>(content)?.Access_Token;
            if (string.IsNullOrWhiteSpace(tokenResponse))
            {
                throw new Exception("PayPal token response did not include an access token.");
            }


            return tokenResponse;

        }

        public async Task<(string, bool)> CreateOrder(CreatePaymentRequist Paymentrequist)
        {
            try
            {
                var environment = _config["PayPal:Environment"];
                var Url = IsSandbox(environment) ?
                    "https://api-m.sandbox.paypal.com/v2/checkout/orders" :
                    "https://api-m.paypal.com/v2/checkout/orders";

                var accessToken = await GetPaymentAccessTokenAsync();

                var request = new HttpRequestMessage(HttpMethod.Post, Url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var total = Paymentrequist.CartItems.Sum(item => item.Price * item.Quantity);
                if (total <= 0)
                {
                    total = Paymentrequist.TotalAmount;
                }

                if (total <= 0)
                {
                    throw new Exception("Payment amount must be greater than zero.");
                }

                var orderItems = Paymentrequist.CartItems.Select(item => new
                {
                    name = item.Name,
                    description = item.Description,

                    unit_amount = new
                    {
                        currency_code = "USD",
                        value = item.Price.ToString("F2")
                    },

                    quantity = item.Quantity.ToString(),

                    category = "PHYSICAL_GOODS",

                    sku = Guid.NewGuid().ToString("N")
                }).ToList();

                var body = new
                {
                    intent = "CAPTURE",
                    purchase_units = new[]
                      {
                          new
                            {
                                amount = new
                                {
                                    currency_code = "USD",
                                    value = total.ToString("F2"),
                                    breakdown = new
                                    {
                                        item_total = new
                                        {
                                            currency_code = "USD",
                                            value = total.ToString("F2")
                                        }
                                    }
                                },
                    
                                items = orderItems
                            }
                        }
                };

                request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return (result, false);
                }

                var json = JsonSerializer.Deserialize<JsonElement>(result);

                if (!json.TryGetProperty("id", out var orderIdProperty))
                {
                    return (result, false);
                }

                var orderId = orderIdProperty.GetString();
                if (string.IsNullOrWhiteSpace(orderId))
                {
                    return (result, false);
                }

                return (orderId, response.IsSuccessStatusCode);

            }
            catch (Exception ex)
            {
                return (ex.Message, false);
            }

        }

        public async Task<(string, bool)> CaptureOrde(string Orderid) 
        {
            try
            {
                var environment = _config["PayPal:Environment"];
                var Url = IsSandbox(environment) ?

                    "https://api-m.sandbox.paypal.com": "https://api-m.paypal.com";

                var accessToken = await GetPaymentAccessTokenAsync();
                var CaptureUrL = $"{Url}/v2/checkout/orders/{Orderid}/capture";

                var request = new HttpRequestMessage(HttpMethod.Post, CaptureUrL);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // اختياري: رجّع التمثيل الكامل للطلب في الرد
                request.Headers.TryAddWithoutValidation("Prefer", "return=representation");
                // مهم: جسم JSON فاضي + Content-Type: application/json
                request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
                // اختياري: لضمان idempotency لو حصل retry
                request.Headers.TryAddWithoutValidation("PayPal-Request-Id", Guid.NewGuid().ToString("N"));


                var response = await _httpClient.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();

                return (result, response.IsSuccessStatusCode);

            }
            catch (Exception ex)
            {
                return (ex.Message, false);
            }

        }

        private static bool IsSandbox(string? environment)
        {
            return string.Equals(environment, "sandbox", StringComparison.OrdinalIgnoreCase);
        }
    }
}
