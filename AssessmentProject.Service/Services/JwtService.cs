using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Net.Http.Json;
using Newtonsoft.Json;
using AssessmentProject.Service.Utilities.Interfaces;
using AssessmentProject.Service.Interfaces;


public class JwtService : IJwtService
{
    private readonly IJwtUtility _jwtUtility;
    private readonly string _secretKey;

    public JwtService(IJwtUtility jwtUtility, IConfiguration configuration)
    {
        _jwtUtility = jwtUtility ?? throw new ArgumentException(nameof(jwtUtility));
        _secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt SecretKey cannot be null");
    }

    public string GenerateJwtToken(string email, int userId, int roleId)
    {
        return _jwtUtility.GenerateJwtToken(email, userId, roleId);
    }

    public string? GetUserIdFromToken(string token)
    {
        return _jwtUtility.GetUserIdFromToken(token);
    }
    public string? GetJWTToken(HttpRequest request)
    {
        _ = request.Cookies.TryGetValue("jwtToken", out string? token);
        return token;
    }

    public string GenerateRefreshToken(int userId, string email, int roleId)
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        var tokenData = new
        {
            UserId = userId,
            Email = email,
            RoleId = roleId,
            RefreshToken = Convert.ToBase64String(randomBytes)
        };

        var tokenString = JsonConvert.SerializeObject(tokenData);
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenString));
    }
    public ClaimsPrincipal? ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = "AssessmentProject",
                ValidAudience = "AssessmentProject",
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }
}
