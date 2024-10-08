using Microsoft.AspNetCore.Identity;

namespace MacroTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        // You can add additional properties here if needed
        public UserProfile UserProfile { get; set; }  // Navigation property for user profile
    }
}
