using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Claim_System.Data;
using System.Linq;

namespace Claim_System.Controllers
{
    [Authorize(Roles = "HR")]
    public class HrController : Controller
    {
        public IActionResult Report()
        {
            var approved = ClaimsStore.Claims
                .Where(c => c.Status == "Approved")
                .ToList();

            return View(approved);
        }
    }
}
