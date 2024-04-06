using Microsoft.AspNetCore.Mvc;

namespace EventManagementApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

   
}
