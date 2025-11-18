using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Claim_System.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            // If the user is NOT logged in, send them to login
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            // If the user is logged in, redirect based on their role
            if (User.IsInRole("Coordinator"))
                return RedirectToAction("Manage", "Claims");

            if (User.IsInRole("HR"))
                return RedirectToAction("Report", "Hr");

            
            return RedirectToAction("Index", "Claims");
        }

        // Error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
