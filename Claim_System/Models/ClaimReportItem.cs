using System;

namespace Claim_System.Models
{
    public class ClaimReportItem
    {
        public string LecturerName { get; set; } = string.Empty;
        public double TotalHours { get; set; }
        public double TotalAmount { get; set; }
        public int ApprovedClaims { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
