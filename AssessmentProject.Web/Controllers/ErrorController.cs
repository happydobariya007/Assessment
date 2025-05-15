using Microsoft.AspNetCore.Mvc;
namespace AssessmentProject.Controllers;

public class ErrorController : Controller
{
    
    [HttpGet]
    public IActionResult NotFound()
    {
        return View();
    }
}
