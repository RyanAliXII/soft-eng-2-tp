using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApp.Areas.Admin.Controllers;
[Authorize]
[Area("Admin")]
public class EventController: Controller{
    public IActionResult Create(){
        return View();
    }
}