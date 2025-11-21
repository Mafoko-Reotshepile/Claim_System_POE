using System;
using System.Collections.Generic;

namespace Claim_System.Models
{
    public class LecturerClaim
    {
        public int Id { get; set; }
        public string LecturerName { get; set; } = string.Empty;
        public string LecturerId { get; set; } = string.Empty;
        public double HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal TotalAmount => (decimal)HoursWorked * HourlyRate;
        public string Description { get; set; } = string.Empty;
        public List<string> UploadedFiles { get; set; } = new List<string>();
        public string Status { get; set; } = "Pending";
        public DateTime SubmittedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
