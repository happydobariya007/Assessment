using AssessmentProject.Repository.Models;
using AssessmentProject.Repository.ViewModels;
using Microsoft.AspNetCore.Http;

namespace AssessmentProject.Service.Interfaces;

public interface IUserService
{
    Task<Users?> Authentication(string email, string password);
    void SetAuthCookies(Users user, HttpResponse response);
    Task<PaginationVM<Users>> GetAllUsers(string sortBy, string sortOrder, int? CurrentPageIndex = null, int? PageSize = null, string? searchKeyword = null);
}
