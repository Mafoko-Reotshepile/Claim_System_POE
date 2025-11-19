namespace Claim_System.Models
{
    public class ClaimReportItem
    {
        public string LecturerName { get; set; } = string.Empty;
        public double TotalHours { get; set; }
        public double TotalAmount { get; set; }
        public int ApprovedClaims { get; set; }
    }
}
