using AssessmentProject.Repository.Interfaces;
using AssessmentProject.Repository.Models;
using AssessmentProject.Repository.ViewModels;
using AssessmentProject.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AssessmentProject.Service.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IStateRepository _stateRepository;
    private readonly ICityRepository _cityRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IJwtService _jwtservice;

    public UserService(IUserRepository userRepository, IJwtService jwtService, ICountryRepository countryRepository, IStateRepository stateRepository, ICityRepository cityRepository, ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _jwtservice = jwtService;
        _countryRepository = countryRepository;
        _stateRepository = stateRepository;
        _cityRepository = cityRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Users?> Authentication(string email, string password)
    {
        return await _userRepository.GetUserByEmailAndPassword(email, password);
    }

    public void SetAuthCookies(Users user, HttpResponse response)
    {
        string token = _jwtservice.GenerateJwtToken(user.EmailId, user.UserID, user.Role.RoleId);

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

    public async Task<(bool isSuccess, string errMessage)> CreateUser(CreateUserVM model)
    {
        bool isContactNumberDuplicate = await _userRepository.CheckDuplicateContactNumber(model.ContactNumber?.ToString());
        bool isEmailDuplicate = await _userRepository.CheckDuplicateEmail(model.EmailId);

        if (isEmailDuplicate)
        {
            return (false, "This EmailId is already exist");
        }
        if (isContactNumberDuplicate)
        {
            return (false, "This Contactnumber is already exist");
        }

        Users? newUser = new Users
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            EmailId = model.EmailId,
            Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
            CountryId = model.CountryId,
            StateId = model.StateId,
            CityId = model.CityId,
            Address = model.Address,
            ContactNumber = model.ContactNumber,
            RoleId = 2, // Role Id 2 for User & 1 for Admin
            CreatedOn = DateTime.UtcNow,
            CreatedBy = _currentUserService.UserId ?? 1
        };
        await _userRepository.CreateUser(newUser);
        return (true, "");

    }

    public async Task<List<Countries>> GetAllCountries()
    {
        return await _countryRepository.GetAllCountries();
    }
    public async Task<List<States>> GetAllStates()
    {
        return await _stateRepository.GetAllStates();
    }
    public async Task<List<Cities>> GetAllCities()
    {
        return await _cityRepository.GetAllCities();
    }
    public async Task<List<States>> GetStatesByCountryId(int countryId)
    {
        return await _stateRepository.GetStatesByCountryId(countryId);
    }
    public async Task<List<Cities>> GetCitiesByStateId(int stateId)
    {
        return await _cityRepository.GetCitiesByStateId(stateId);
    }
}
