using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Claim_System.Models;

namespace Claim_System.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string username = model.Username?.Trim().ToLower() ?? "";
            string password = model.Password?.Trim() ?? "";

            // Demo hardcoded users (lowercase usernames)
            bool isLecturer = username == "lecturer" && password == "123";
            bool isCoordinator = username == "coordinator" && password == "123";
            bool isHR = username == "hr" && password == "123";

            if (!isLecturer && !isCoordinator && !isHR)
            {
                ViewBag.Error = "Invalid username or password.";
                return View(model);
            }

            string role = isCoordinator ? "Coordinator" : isHR ? "HR" : "Lecturer";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProps = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = System.DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return LocalRedirect(model.ReturnUrl);

            return role switch
            {
                "Coordinator" => RedirectToAction("CoordinatorDashboard", "Home"),
                "HR" => RedirectToAction("HrDashboard", "Home"),
                _ => RedirectToAction("LecturerDashboard", "Home")
            };
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
