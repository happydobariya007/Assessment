using AssessmentProject.Repository.Models;
using AssessmentProject.Repository.ViewModels;
using AssessmentProject.Service.Attributes;
using AssessmentProject.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentProject.Web.Controllers;

public class ConcertController : Controller
{
    private readonly IConcertService _concertService;
    public ConcertController(IConcertService concertService)
    {
        _concertService = concertService;
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

    [Route("Concert/Concertlist")]
    [CustomAuthorize("Admin", "User")]
    [HttpGet]
    public async Task<IActionResult> Concertlist(string sortBy, string sortOrder, int currentPage = 1, int pageSize = 10, string searchKeyword = "")
    {
        try
        {
            string? token = Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }
            sortBy = string.IsNullOrEmpty(sortBy) ? "userId" : sortBy;
            sortOrder = string.IsNullOrEmpty(sortOrder) ? "userId" : sortOrder;

            PaginationVM<Concerts>? concerts = await _concertService.GetAllConcerts(currentPage, pageSize, searchKeyword);

            ViewBag.PageSize = pageSize;
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                ViewBag.searchKeyword = searchKeyword;
                return PartialView("_ConcertListPartial", concerts);
            }

            return View(concerts);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Error getting concerts: " + ex.Message });
        }
    }

    /// <summary>
    /// This method is get method and used for create new concert.
    /// </summary>
    /// <remarks>
    /// This method provide form by which can create new concert.
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
    [Route("Concert/CreateConcert")]
    public IActionResult CreateConcert()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Error Fetching create concert form: " + ex.Message });
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
    [Route("Concert/CreateConcert")]
    public async Task<IActionResult> CreateConcert(CreateConcertVM model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data provided";
                return View(model);
            }
            (bool isCreated, string errMessage) = await _concertService.CreateConcert(model);
            if (!isCreated)
            {
                TempData["Error"] = !ModelState.IsValid ? "Invalid data provided" : errMessage;
                return View(model);
            }
            else
            {
                TempData["Success"] = "Concert created successfully";
                return RedirectToAction("Concertlist", "Concert");
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Login", "User");
        }
    }

    /// <summary>
    /// This method is get method and used for edit existing concertdetails.
    /// </summary>
    /// <remarks>
    /// This method fetched data from viewModel and fill into the formdata of the view.
    /// </remarks>
    /// <param name="concertId">userId for edit data of particular concert.</param>
    /// <returns>
    /// It returns the editConcert view for update details.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when there is an error while editing existing concert such as a database connection failure.
    /// </exception>
    /// <author>Happy Dobariya</author>
    /// <modifiedby>Happy Dobariya</modifiedby>
    /// <date>2025-05-15</date>
    [HttpGet]
    [Route("Concert/EditConcert")]
    public async Task<IActionResult> EditConcert(int concertId)
    {
        try
        {

            Concerts? concert = await _concertService.GetConcertById(concertId);
            if (concert == null)
            {
                TempData["Error"] = "concert does not exist with this concertId!";
                return RedirectToAction("Concertlist", "Concert");
            }

            if (concert.Isdeleted)
            {
                TempData["Error"] = "You can't edit deleted concerts!";
                return RedirectToAction("Concertlist", "Concert");
            }

            EditConcertVM? model = await _concertService.ConcertDataForUpdate(concertId);
            if (model == null)
            {
                return RedirectToAction("Concertlist", "Concert");
            }
            model.ConcertID = concertId;
            return View(model);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Error getting concert detail: " + ex.Message });
        }
    }

    /// <summary>
    /// This method is post method and used for edit existing concertdetails.
    /// </summary>
    /// <remarks>
    /// This method is used for edit concertdetails.
    /// </remarks>
    /// <param name="EditConcertVM model">ViewModel for update data of the user.</param>
    /// <returns>
    /// It is redirected to the concertlist page after edit the concertdetails.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when there is an error while editing ecisting concert such as a database connection failure.
    /// </exception>
    /// <author>Happy Dobariya</author>
    /// <modifiedby>Happy Dobariya</modifiedby>
    /// <date>2025-05-15</date>
    [HttpPost]
    [Route("Concert/EditConcert")]
    public async Task<IActionResult> EditConcert(EditConcertVM model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            (bool isUpdated, string errMessage) = await _concertService.UpdateConcert(model);
            if (!isUpdated)
            {
                TempData["Error"] = errMessage;
                return RedirectToAction("EditConcert", "Concert", model);
            }
            TempData["Success"] = "Concert updated successfully";
            return RedirectToAction("Concertlist", "Concert");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Concertlist", "Concert", model);
        }
    }

    /// <summary>
    /// This method is post method and used for soft delete the concert.
    /// </summary>
    /// <remarks>
    /// This method fetched concertId from token and softdelete the concert.
    /// </remarks>
    /// <param name="concertId">concertId for softdelete the data of the concert.</param>
    /// <returns>
    /// It is redirected to the concertlist page after delete the concert.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when there is an error while creating new concert such as a database connection failure.
    /// </exception>
    /// <author>Happy Dobariya</author>
    /// <modifiedby>Happy Dobariya</modifiedby>
    /// <date>2025-05-15</date>
    [HttpPost]
    public IActionResult DeleteConcert(int concertId)
    {
        try
        {
            _concertService.DeleteConcert(concertId);
            TempData["Success"] = "Concert Deleted Successfully";
            return RedirectToAction("Concertlist", "Concert");
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Error deleting concert: " + ex.Message });
        }
    }
    /// <summary>
    /// This method handles the GET request to fetch and display detailed information for a specific concert.
    /// </summary>
    /// <remarks>
    /// It retrieves the concert details from the database using the provided `concertId` 
    /// and returns a view displaying all the details of that concert.
    /// </remarks>
    /// <param name="concertId">The ID of the concert for which the details are to be fetched.</param>
    /// <returns>
    /// Returns a view displaying the details of the specified concert.
    /// </returns>
    /// <author>Happy Dobariya</author>
    /// <modifiedby>Happy Dobariya</modifiedby>
    /// <date>2025-05-15</date>

    [HttpGet]
    [Route("Concert/ConcertDetails")]
    public async Task<IActionResult> ConcertDetails(int concertId)
    {
        try
        {
            Concerts? concertDetail = await _concertService.GetConcertById(concertId);
            return View(concertDetail);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "An error occurred while fetching the order details", error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> BookTickets(int? concertId)
    {
        try
        {
            List<Concerts>? concerts = await _concertService.GetEveryConcerts();
            BookTicketsVM bookTicketsVM = new BookTicketsVM
            {
                ConcertID = concertId ?? 0
            };
            ViewBag.Concerts = concerts;
            return PartialView("_BookTicketsPartial", bookTicketsVM);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Error loading concerts for booking: " + ex.Message });
        }
    }

    [HttpGet]
    public async Task<int> GetPriceByConcertId(int concertId)
    {
        try
        {
            return _concertService.GetPriceByConcertId(concertId);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return 0;
        }
    }
    [HttpGet]
    public async Task<int> GetRequiredTicketsByConcertId(int concertId)
    {
        try
        {
            return _concertService.GetPriceByConcertId(concertId);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return 0;
        }
    }
    [HttpGet]
    public async Task<int> GetDiscountByConcertId(int concertId)
    {
        try
        {
            return _concertService.GetPriceByConcertId(concertId);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return 0;
        }
    }


}
