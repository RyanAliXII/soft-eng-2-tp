using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApp.Areas.Admin.Controllers
{   [Authorize]
    [Area("Admin")]
    public class DashboardController: Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
