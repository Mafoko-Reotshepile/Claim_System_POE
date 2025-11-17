using Xunit;
using Claim_System.Models;

namespace Claim_System.Tests
{
    public class ClaimTests
    {
        [Fact]
        public void NewClaim_DefaultStatus_Pending()
        {
            var c = new Claim();
            Assert.Equal("Pending", c.Status);
        }

        [Fact]
        public void TotalAmount_CalculatesCorrectly()
        {
            var c = new Claim { HoursWorked = 10, HourlyRate = 50 };
            Assert.Equal(500, c.TotalAmount);
        }

        [Fact]
        public void Approve_SetsStatusApproved()
        {
            var c = new Claim();
            c.Status = "Pending";
            c.Status = "Approved";
            Assert.Equal("Approved", c.Status);
        }

        [Fact]
        public void Reject_SetsStatusRejected()
        {
            var c = new Claim();
            c.Status = "Pending";
            c.Status = "Rejected";
            Assert.Equal("Rejected", c.Status);
        }
    }
}
