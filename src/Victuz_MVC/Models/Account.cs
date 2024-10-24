using Microsoft.AspNetCore.Identity;

namespace Victuz_MVC.Models
{
    public class Account : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public ICollection<Activity> Activities { get; } = [];

        public ICollection<Order> Orders { get; } = [];
    }
}
