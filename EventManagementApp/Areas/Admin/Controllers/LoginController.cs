using System.Security.Claims;
using EventManagementApp.Areas.Admin.ViewModels;
using EventManagementApp.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public LoginController(ILogger<LoginController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel credential)
        {
            try
            {
                ViewData["error"] = "";
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return View(credential);
                }
                var user = await _unitOfWork.UserRepository.GetUserByEmail(credential.Email);
                if (user == null)
                {
                    ViewData["error"] = "Invalid username or password";
                    _logger.LogInformation("User not found.");
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return View(credential);
                }
                var dbPassword = user.LoginCredential.Password;
                var isCorrectPassword = BCrypt.Net.BCrypt.EnhancedVerify(credential.Password, dbPassword);
                if (!isCorrectPassword)
                {
                    ViewData["error"] = "Invalid username or password";
                    _logger.LogInformation("Incorrect password.");
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return View(credential);
                }
                var claims = new List<Claim>{
                    new ("Id", user.Id.ToString()),
                    new ("GivenName", user.GivenName),
                    new ("MiddleName", user.MiddleName),
                    new("Surname", user.Surname),
                    new("Email", user.LoginCredential.Email),
                    new("LoginCredentialId", user.LoginCredentialId.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
                return Redirect("/Admin/Dashboard");
            }
            catch (Exception e)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                ViewData["error"] = "Unknown error occured, please try again later.";
                _logger.LogError(e.Message);
                return View(credential);
            }
        }

    }
}
