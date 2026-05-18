using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using BusinessLayer.Helpers;
using DataAccessLayer.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace BusinessLayer.Services
{
    public class TokenService : ITokenService   
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUserService _userService;
        public TokenService(IOptions<JwtSettings> jwtSettings, IUserService userService )
        {
            _jwtSettings = jwtSettings.Value;
            _userService = userService;
        }
        public async Task<string> CreateAccessTokenAsync(DTOUser user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key)
            );

            var Claims = await _userService.GetClaims(user.UserName);

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: Claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );
            token.ToString();
            return new JwtSecurityTokenHandler().WriteToken(token);
         /*string text2 = "Hello World";
            string secretKey = "MySecret";

            // تحويل المفتاح والنص إلى bytes
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] textBytes = Encoding.UTF8.GetBytes(text2);

            // حساب التوقيع
            using (var hmac = new HMACSHA256(keyBytes))
            {
                byte[] signatureBytes = hmac.ComputeHash(textBytes);
                string signature = Convert.ToHexString(signatureBytes);

                Console.WriteLine(signature);
                // Output: 334D41A3D9B53B50B5B2A3B4C5D6E7F8...
           }*/
        }
        public string CreateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}
