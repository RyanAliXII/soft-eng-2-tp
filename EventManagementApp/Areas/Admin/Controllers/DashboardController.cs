using Microsoft.AspNetCore.Mvc;

namespace EventManagementApp.Areas.Admin.Controllers
{   
    [Area("Admin")]
    public class DashboardController: Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
