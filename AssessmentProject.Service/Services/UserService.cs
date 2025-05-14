using AssessmentProject.Repository.Interfaces;
using AssessmentProject.Repository.Models;
using AssessmentProject.Repository.ViewModels;
using AssessmentProject.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using AssessmentProject.Service.Interfaces;

namespace AssessmentProject.Service.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtservice;

    public UserService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtservice = jwtService;
    }

    public async Task<Users?> Authentication(string email, string password)
    {
        return await _userRepository.GetUserByEmailAndPassword(email, password);
    }

    public void SetAuthCookies(Users user, HttpResponse response)
    {
        string token = _jwtservice.GenerateJwtToken(user.EmailId, user.UserID, user.Roles.RoleId);

        CookieOptions cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(30),
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.Strict
        };

        response.Cookies.Append("jwtToken", token, cookieOptions);
    }

    public async Task<PaginationVM<Users>> GetAllUsers(string sortBy, string sortOrder, int? CurrentPageIndex = null, int? PageSize = null, string? searchKeyword = null)
    {
        int currentPage = CurrentPageIndex ?? 1;
        int pageSize = PageSize ?? 10;
        var users = await _userRepository.GetAllUsers(sortBy, sortOrder, searchKeyword);
        var totalUsers = users.Count();
        var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);
        users = users.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        PaginationVM<Users> userListPagination = new PaginationVM<Users>();
        userListPagination.Data = users;
        userListPagination.CurrentPageIndex = currentPage;
        userListPagination.PageSize = pageSize;
        userListPagination.TotalPages = totalPages;
        userListPagination.TotalUsers = totalUsers;
        return userListPagination;
    }
}
