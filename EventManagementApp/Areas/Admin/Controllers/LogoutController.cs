using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EventManagementApp.Areas.Admin.Controllers;
[Authorize]
[Area("Admin")]
public class LogoutController: Controller {
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult>Index(){
         await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/Admin/Login");
    }

}