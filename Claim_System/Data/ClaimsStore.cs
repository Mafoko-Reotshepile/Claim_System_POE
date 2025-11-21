using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Claim_System.Data
{
    using Claim_System.Models;

    public static class ClaimsStore
    {
        //private static readonly List<LecturerClaim> _claims = new();

        public static List<LecturerClaim> Claims { get; } = new List<LecturerClaim>();
        public static int NextId = 1;
        private static readonly object _lock = new();

        // Thread-safe ID generation
        public static int GetNextId()
        {
            return Interlocked.Increment(ref NextId);
        }

        // Add a new claim
        public static void AddClaim(LecturerClaim claim)
        {
            if (claim == null) throw new ArgumentNullException(nameof(claim));
            claim.Id = GetNextId();
            lock (_lock)
            {
                Claims.Add(claim);
            }
        }

        // Update an existing claim
        public static void UpdateClaim(LecturerClaim claim)
        {
            if (claim == null) throw new ArgumentNullException(nameof(claim));

            lock (_lock)
            {
                var index = Claims.FindIndex(c => c.Id == claim.Id);
                if (index >= 0)
                {
                    Claims[index] = claim;
                }
            }
        }

        // Get a claim by ID
        public static LecturerClaim GetClaim(int id)
        {
            lock (_lock)
            {
                return Claims.FirstOrDefault(c => c.Id == id);
            }
        }

        // Get all claims (for Coordinator)
        public static List<LecturerClaim> GetAllClaims()
        {
            lock (_lock)
            {
                return Claims.OrderByDescending(c => c.SubmittedAt).ToList();
            }
        }

        // Get claims by lecturer name
        public static List<LecturerClaim> GetClaimsByLecturer(string lecturerName)
        {
            if (string.IsNullOrWhiteSpace(lecturerName)) return new List<LecturerClaim>();

            lock (_lock)
            {
                return Claims
                    .Where(c => c.LecturerName.Equals(lecturerName, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(c => c.SubmittedAt)
                    .ToList();
            }
        }
    }
}
