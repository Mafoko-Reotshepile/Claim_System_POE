using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Claim_System.Models;
using Claim_System.Data;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Claim_System.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        private static readonly string[] AllowedExtensions = { ".pdf", ".doc", ".docx", ".xlsx" };
        private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public ClaimsController()
        {
            if (!Directory.Exists(_uploadFolder))
                Directory.CreateDirectory(_uploadFolder);
        }

        [Authorize(Roles = "Lecturer")]
        public IActionResult Index()
        {
            string currentUser = User.Identity?.Name ?? string.Empty;
            var myClaims = ClaimsStore.Claims
                .Where(c => string.Equals(c.LecturerName, currentUser, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return View(myClaims);
        }

        [Authorize(Roles = "Lecturer")]
        [HttpGet]
        public IActionResult Create() => View(new SubmitClaimViewModel());

        [Authorize(Roles = "Lecturer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubmitClaimViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Validate files first
            var savedFiles = new List<string>();
            if (model.Files != null && model.Files.Count > 0)
            {
                foreach (var file in model.Files)
                {
                    if (file == null || file.Length == 0) continue;

                    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!AllowedExtensions.Contains(ext))
                    {
                        ModelState.AddModelError("Files", $"File type {ext} not allowed.");
                        continue;
                    }

                    if (file.Length > 10 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Files", $"File {file.FileName} exceeds 10 MB limit.");
                        continue;
                    }
                }

                if (!ModelState.IsValid)
                    return View(model);

                // Save files
                foreach (var file in model.Files)
                {
                    if (file == null || file.Length == 0) continue;
                    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                    var unique = $"{Guid.NewGuid():N}{ext}";
                    var relPath = Path.Combine("uploads", unique).Replace("\\", "/");
                    var physical = Path.Combine(_uploadFolder, unique);
                    using (var stream = new FileStream(physical, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    savedFiles.Add(relPath);
                }
            }

            // Thread-safe id generation
            int id = Interlocked.Increment(ref ClaimsStore.NextId);
            var claim = new LecturerClaim
            {
                Id = id,
                LecturerName = User.Identity?.Name ?? "",
                Description = model.Description,
                HoursWorked = model.HoursWorked,
                HourlyRate = model.HourlyRate,
                SubmittedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                UploadedFiles = savedFiles
            };

            ClaimsStore.Claims.Add(claim);

            TempData["SuccessMessage"] = "Claim submitted successfully!";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Coordinator")]
        public IActionResult Manage()
        {
            var claims = ClaimsStore.Claims
                .OrderBy(c => c.Status)
                .ThenByDescending(c => c.SubmittedAt)
                .ToList();
            return View(claims);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var claim = ClaimsStore.Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) return NotFound();

            if (User.IsInRole("Lecturer") && !string.Equals(User.Identity?.Name, claim.LecturerName, StringComparison.OrdinalIgnoreCase))
                return Forbid();

            return View(claim);
        }

        [HttpGet]
        public IActionResult DownloadFile(int id, string file)
        {
            var claim = ClaimsStore.Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null || !claim.UploadedFiles.Any())
                return NotFound();

            var matched = claim.UploadedFiles.FirstOrDefault(f => Path.GetFileName(f).Equals(file, StringComparison.OrdinalIgnoreCase));
            if (matched == null) return Forbid();

            var physical = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", matched.Replace('/', Path.DirectorySeparatorChar));
            if (!System.IO.File.Exists(physical)) return NotFound();

            var ext = Path.GetExtension(physical);
            var contentType = ext switch
            {
                ".pdf" => "application/pdf",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".doc" => "application/msword",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream"
            };

            var bytes = System.IO.File.ReadAllBytes(physical);
            return File(bytes, contentType, Path.GetFileName(physical));
        }

        [Authorize(Roles = "Coordinator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            var claim = ClaimsStore.Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) { TempData["ErrorMessage"] = "Claim not found."; return RedirectToAction(nameof(Manage)); }

            claim.Status = "Approved";
            claim.LastUpdatedAt = DateTime.UtcNow;
            TempData["SuccessMessage"] = $"Claim {id} approved.";
            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = "Coordinator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id)
        {
            var claim = ClaimsStore.Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) { TempData["ErrorMessage"] = "Claim not found."; return RedirectToAction(nameof(Manage)); }

            claim.Status = "Rejected";
            claim.LastUpdatedAt = DateTime.UtcNow;
            TempData["SuccessMessage"] = $"Claim {id} rejected.";
            return RedirectToAction(nameof(Manage));
        }
    }
}
