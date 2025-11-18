using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Claim_System.Models;
using System.Linq;

namespace Claim_System.Controllers
{
    [Authorize(Roles = "HR")]
    public class HrController : Controller
    {
        public IActionResult Report()
        {
            var approved = ClaimsControllerData.Claims
                .Where(c => c.Status == "Approved")
                .ToList();

            return View(approved);
        }
    }

    // Shared claim data store for HR
    public static class ClaimsControllerData
    {
        public static List<Claim> Claims => Claim_System.Controllers.ClaimsController._claims;
    }
}
