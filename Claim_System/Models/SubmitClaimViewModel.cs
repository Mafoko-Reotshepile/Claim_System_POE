using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Claim_System.Models
{
    public class SubmitClaimViewModel
    {
        [Required(ErrorMessage = "Hours worked is required")]
        [Range(0.1, 1000, ErrorMessage = "Hours must be greater than 0")]
        public double HoursWorked { get; set; }

        [Required(ErrorMessage = "Hourly rate is required")]
        [Range(0.1, 10000, ErrorMessage = "Rate must be greater than 0")]
        public double HourlyRate { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        // Multiple file uploads
        public List<IFormFile>? Files { get; set; }
    }
}
