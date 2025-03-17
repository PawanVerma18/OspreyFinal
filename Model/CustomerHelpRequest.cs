using System;

namespace Osprey3.Models
{
    public class CustomerHelpRequest
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IssueDescription { get; set; }
        public DateTime RequestedOn { get; set; } = DateTime.UtcNow;
    }
}