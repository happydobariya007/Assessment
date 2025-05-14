using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using AssessmentProject.Service.Utilities.Interfaces;

namespace AssessmentProject.Service.Utilities
{
    public class JwtUtility : IJwtUtility
    {
        private readonly string _secretKey;
        private readonly double _tokenExpirationInHours;

        public JwtUtility(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt SecretKey cannot be null");
            _tokenExpirationInHours = configuration.GetValue<double>("Jwt:TokenExpirationInHours", 1);
        }

        public string GenerateJwtToken(string email, int userId, int roleId)
        {
            Claim[]? claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, roleId.ToString())
            };

            SymmetricSecurityKey? key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            SigningCredentials? creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken? token = new JwtSecurityToken(
                issuer: "AssessmentProject",
                audience: "AssessmentProject",
                claims: claims,
                expires: DateTime.Now.AddHours(_tokenExpirationInHours),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string? GetUserIdFromToken(string token)
        {
            JwtSecurityTokenHandler? handler = new JwtSecurityTokenHandler();
            JwtSecurityToken? jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken != null)
            {
                Claim? userIdClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    return userIdClaim.Value;
                }
            }

            return null;
        }
    }
}
