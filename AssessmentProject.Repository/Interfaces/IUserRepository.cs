using AssessmentProject.Repository.Models;

namespace AssessmentProject.Repository.Interfaces;

public interface IUserRepository
{
    Task<Users?> GetUserByEmailAndPassword(string email, string password);
    Task<List<Users>> GetAllUsers(string sortBy, string sortOrder, string? searchKeyword = null);
}
