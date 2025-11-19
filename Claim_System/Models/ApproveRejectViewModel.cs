using System.ComponentModel.DataAnnotations;

namespace Claim_System.Models
{
    public class ApproveRejectViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [RegularExpression("Approved|Rejected", ErrorMessage = "Invalid action")]
        public string Action { get; set; } = string.Empty;
    }
}
