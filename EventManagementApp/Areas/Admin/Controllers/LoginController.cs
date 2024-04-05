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

    }
}
