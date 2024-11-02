using Microsoft.AspNetCore.Identity;

namespace Victuz_MVC.Models
{
    public class Account : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // Hosted activities
        public ICollection<Activity> Activities { get; } = [];

        // Enrolled activities
        public ICollection<Enrollment> Enrollments { get; } = [];

        public ICollection<Order> Orders { get; } = [];

        public bool Blacklisted { get; set; }
    }
}
