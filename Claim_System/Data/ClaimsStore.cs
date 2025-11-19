using System.Collections.Generic;
using Claim_System.Models;

namespace Claim_System.Data
{
    public static class ClaimsStore
    {
        public static readonly List<LecturerClaim> Claims = new List<LecturerClaim>();
        public static int NextId = 1;
    }
}
