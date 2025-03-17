using System;
using System.Collections.Generic;

namespace Osprey3.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Optional fields for password recovery
        public string VerificationCode { get; set; } // No [Required] attribute
        public DateTime? VerificationCodeExpiry { get; set; } // No [Required] attribute
    }
}