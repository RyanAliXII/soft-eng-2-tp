using EventManagementApp.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApp.Areas.Admin.Controllers
{   
    [Area("Admin")]
    public class LoginController: Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel creds){
        
            return View();
        }

    }
}
