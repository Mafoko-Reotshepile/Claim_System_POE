using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Claim_System.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl)
        {
            username = username?.Trim().ToLower()??"";
            password = password?.Trim()??"";

            bool isLecturer = username == "lecturer" && password == "123";
            bool isCoordinator = username == "coordinator" && password == "123";
            bool isHR = username == "hr" && password == "123";


            if (!isLecturer && !isCoordinator && !isHR)
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }

            var role = isCoordinator ? "Coordinator" : isHR ? "HR" : "Lecturer";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, isCoordinator ? "Coordinator": isHR ? "HR" : "Lecturer")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (!string.IsNullOrEmpty(returnUrl))
                return LocalRedirect(returnUrl);

            if (isCoordinator)
                return RedirectToAction("Manage", "Claims");

            if (isHR)
                return RedirectToAction("Report", "Hr");

            return RedirectToAction("Index", "Claims");
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
