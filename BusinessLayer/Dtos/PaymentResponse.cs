using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BusinessLayer.Dtos
{
    public class PayPalTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string Access_Token { get; set; } = string.Empty;

        [JsonPropertyName("token_type")]
        public string Token_Type { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int Expires_In { get; set; }
    }
}
