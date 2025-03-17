using System;

namespace Osprey3.Models
{
    public class TalentApplication
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string TalentName { get; set; }
        public string Description { get; set; }
        public string SocialMediaLink { get; set; }
        public DateTime AppliedOn { get; set; }
    }
}