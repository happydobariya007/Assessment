using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace AssessmentProject.Service.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(string email, int userId, int roleId);
    string? GetUserIdFromToken(string token);
    string? GetJWTToken(HttpRequest request);
    string GenerateRefreshToken(int userId, string email, int roleId);
    ClaimsPrincipal? ValidateToken(string token);
}
