namespace AssessmentProject.Service.Utilities.Interfaces;

public interface IJwtUtility
{
    string GenerateJwtToken(string email, int userId, int roleId);
    string? GetUserIdFromToken(string token);
}
