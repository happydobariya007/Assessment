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


    public UserController(IUserService userService, IJwtUtility jwtUtility)
    {
        _userService = userService;
        _jwtUtility = jwtUtility;
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
            if (Request.Cookies["userEmail"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
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
                TempData["Error"] = "Invalid email or password";
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
