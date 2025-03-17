using System;

namespace Osprey3.Models
{
    public class Funding
    {
        public int Id { get; set; }
        public decimal TotalFunding { get; set; }
        public decimal ContributionAmount { get; set; }
        public string ContributorEmail { get; set; } // Email of the user who contributed
        public DateTime ContributionDate { get; set; } = DateTime.UtcNow;
    }
}