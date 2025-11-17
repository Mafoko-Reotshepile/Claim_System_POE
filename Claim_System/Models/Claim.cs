using System;
using System.Collections.Generic;

namespace Claim_System.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public string LecturerName { get; set; } = string.Empty;
        public string LecturerId { get; set; } = string.Empty;
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string? FileName { get; set; }
        public string UploadedFiles { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public double TotalAmount => Math.Round(HoursWorked * HourlyRate, 2);
    }
}
