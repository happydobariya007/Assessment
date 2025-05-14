using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AssessmentProject.Web.Models;
using AssessmentProject.Service.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace AssessmentProject.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [CustomAuthorize("Admin")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
