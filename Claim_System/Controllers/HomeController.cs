using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Claim_System.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
                if (User.IsInRole("Coordinator")) return RedirectToAction("CoordinatorDashboard");
                if (User.IsInRole("HR")) return RedirectToAction("HrDashboard");
                return RedirectToAction("LecturerDashboard");
            }

            return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = "Lecturer")]
        public IActionResult LecturerDashboard() => View();

        [Authorize(Roles = "Coordinator")]
        public IActionResult CoordinatorDashboard() => View();

        [Authorize(Roles = "HR")]
        public IActionResult HrDashboard() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View();
    }
}
