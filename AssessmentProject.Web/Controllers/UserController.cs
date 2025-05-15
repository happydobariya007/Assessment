using System.Security.Claims;
using AssessmentProject.Repository.Models;
using AssessmentProject.Repository.ViewModels;
using AssessmentProject.Service.Attributes;
using AssessmentProject.Service.Interfaces;
using AssessmentProject.Service.Utilities.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentProject.Web.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IJwtUtility _jwtUtility;
    private readonly IRoleService _roleService;


    public UserController(IUserService userService, IJwtUtility jwtUtility, IRoleService roleService)
    {
        _userService = userService;
        _jwtUtility = jwtUtility;
        _roleService = roleService;
    }

    /// <summary>
    /// This method is get method and used for return loginpage.
    /// </summary>
    /// <remarks>
    /// This method display the view of the login page with form for login.
    /// </remarks>
    /// <param name=""></param>
    /// <returns>
    /// It returns the login page view.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when there is an error while creating new user such as a database connection failure.
    /// </exception>
    /// <author>Happy Dobariya</author>
    /// <modifiedby>Happy Dobariya</modifiedby>
    /// <date>2025-05-14</date>
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        try
        {
            Response.Cookies.Delete("jwtToken");
            return View();
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View();
        }
    }

    /// <summary>
    /// This method is post method and used for submit the login form.
    /// </summary>
    /// <remarks>
    /// This method calls the Authentication method from service.
    /// </remarks>
    /// <param name="LoginVM model">LoginVM model is used for submit login form</param>
    /// <returns>
    /// It returns the dashboard page after successfull login otherwise return the login view.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when there is an error while creating new user such as a database connection failure.
    /// </exception>
    /// <author>Happy Dobariya</author>
    /// <modifiedby>Happy Dobariya</modifiedby>
    /// <date>2025-05-14</date>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginVM model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Users? user = await _userService.Authentication(model.Email, model.Password);

            if (user == null)
            {
                TempData["Error"] = "Invalid email or password If you not registered yet, please register first!";
                return View();
            }
            if (user.Isdeleted)
            {
                TempData["Error"] = "User with this email does not exist";
                return View();
            }
            _userService.SetAuthCookies(user, Response);
            TempData["Success"] = "Login Successfull";
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View();
        }
    }

    /// <summary>
    /// This method is get method and used for listing all the user.
    /// </summary>
    /// <remarks>
    /// This method fetches all users and show them on userlist view and provide functionality for sorting and searching.
    /// </remarks>
    /// <param name="sortBy,sortOrder,currentPag,pageSize,searchKeyword">used for fetched data from database via service and repository</param>
    /// <returns>
    /// Returned the view with all users.
    /// </returns>
    /// <exception cref="Exception">
    /// </exception>
    /// <author>Happy Dobariya</author>
    /// <modifiedby>Happy Dobariya</modifiedby>
    /// <date>2025-05-14</date>

    [Route("User/Userlist")]
    [CustomAuthorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> Userlist(string sortBy, string sortOrder, int currentPage = 1, int pageSize = 10, string searchKeyword = "")
    {
        try
        {
            string? token = Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            string? userId = _jwtUtility.GetUserIdFromToken(token);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home");
            }
            sortBy = string.IsNullOrEmpty(sortBy) ? "userId" : sortBy;
            sortOrder = string.IsNullOrEmpty(sortOrder) ? "userId" : sortOrder;

            PaginationVM<Users>? users = await _userService.GetAllUsers(sortBy, sortOrder, currentPage, pageSize, searchKeyword);

            ViewBag.PageSize = pageSize;
            ViewBag.userId = userId;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                ViewBag.searchKeyword = searchKeyword;
                return PartialView("_UserListPartial", users);
            }

            return View(users);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Error getting users: " + ex.Message });
        }
    }

    /// <summary>
    /// This method is get method and used for create new user.
    /// </summary>
    /// <remarks>
    /// This method provide form by which can create new user.
    /// </remarks>
    /// <param name=""></param>
    /// <returns>
    /// Returned the view with form.
    /// </returns>
    /// <exception cref="Exception">
    /// </exception>
    /// <author>Happy Dobariya</author>
    /// <modifiedby>Happy Dobariya</modifiedby>
    /// <date>2025-05-15</date>
    [HttpGet]
    [Route("User/CreateUser")]
    public async Task<IActionResult> CreateUser()
    {
        try
        {
            ViewBag.Countries = await _userService.GetAllCountries();
            return View();
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Error Fetching createuser form: " + ex.Message });
        }
    }

    /// <summary>
    /// This method is post method and used for create new user.
    /// </summary>
    /// <remarks>
    /// This method fetched data from viewModel and fill into the model.
    /// </remarks>
    /// <param name="CreateUserVM model">ViewModel for create new user.</param>
    /// <returns>
    /// It is redirected to the userlist page after create user.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when there is an error while creating new user such as a database connection failure.
    /// </exception>
    /// <author>Happy Dobariya</author>
    /// <modifiedby>Happy Dobariya</modifiedby>
    /// <date>2025-03-03</date>
    // [Authorize(Roles = "Admin, Chef, Account Manager")]
    [HttpPost]
    [Route("User/CreateUser")]
    public async Task<IActionResult> CreateUser(CreateUserVM model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data provided";
                ViewBag.Roles = await _roleService.GetAllRoles();
                ViewBag.Countries = await _userService.GetAllCountries();
                return View(model);
            }
            (bool isCreated, string errMessage) = await _userService.CreateUser(model);
            if (!isCreated)
            {
                TempData["Error"] = !ModelState.IsValid ? "Invalid data provided" : errMessage;

                ViewBag.Roles = await _roleService.GetAllRoles();
                ViewBag.Countries = await _userService.GetAllCountries();

                if (model.CountryId.HasValue)
                {
                    ViewBag.States = await _userService.GetStatesByCountryId(model.CountryId.Value);
                }

                if (model.StateId.HasValue)
                {
                    ViewBag.Cities = await _userService.GetCitiesByStateId(model.StateId.Value);
                }

                return View(model);
            }
            TempData["Success"] = "Registration completed successfully";
            return RedirectToAction("Login", "User");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Login", "User");
        }
    }
    /// <summary>
    /// This method is get method and used for listing all states.
    /// </summary>
    /// <remarks>
    /// This method listing all states according particular countryId.
    /// </remarks>
    /// <param name="countryId">countryId used for filer states data</param>
    /// <returns>
    /// It returns the Json of all states according to particular countryId.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when there is an error while creating new user such as a database connection failure.
    /// </exception>
    /// <author>Happy Dobariya</author>
    /// <modifiedby>Happy Dobariya</modifiedby>
    /// <date>2025-05-15</date>
    [HttpGet]
    public async Task<JsonResult> GetStates(int countryId)
    {
        try
        {
            List<States>? states = await _userService.GetStatesByCountryId(countryId);
            var stateList = states.Select(s => new { s.Id, s.StateName }).ToList();
            return Json(stateList);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "An error occurred while fetching states.", error = ex.Message });
        }
    }

    /// <summary>
    /// This method is get method and used for listing all cities.
    /// </summary>
    /// <remarks>
    /// This method listing all cities according particular stateId.
    /// </remarks>
    /// <param name="stateId">stateId used for filer cities data</param>
    /// <returns>
    /// It returns the Json of all cities according to particular stateId.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when there is an error while creating new user such as a database connection failure.
    /// </exception>
    /// <author>Happy Dobariya</author>
    /// <modifiedby>Happy Dobariya</modifiedby>
    /// <date>2025-05-15</date>
    [HttpGet]
    public async Task<JsonResult> GetCities(int stateId)
    {
        try
        {
            List<Cities>? cities = await _userService.GetCitiesByStateId(stateId);
            var citiesList = cities.Select(c => new { c.Id, c.CityName }).ToList();
            return Json(citiesList);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "An error occurred while fetching cities.", error = ex.Message });
        }
    }

    /// <summary>
    /// This method is post method and used for logout.
    /// </summary>
    /// <remarks>
    /// This method is used for clear cookies.
    /// </remarks>
    /// <param name=""></param>
    /// <returns>
    /// It returns the login view.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when there is an error while creating new user such as a database connection failure.
    /// </exception>
    /// <author>Happy Dobariya</author>
    /// <modifiedby>Happy Dobariya</modifiedby>
    /// <date>2025-05-14</date>
    [HttpPost]
    [Route("User/Logout")]
    public IActionResult Logout()
    {
        try
        {
            Response.Cookies.Delete("jwtToken");
            TempData["Success"] = "Logout Successful";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Login", "User");
    }


}
