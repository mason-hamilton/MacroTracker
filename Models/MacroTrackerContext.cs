using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MacroTracker.Models
{
    public class MacroTrackerContext : IdentityDbContext<ApplicationUser>
    {
        public MacroTrackerContext(DbContextOptions<MacroTrackerContext> options)
            : base(options) { }

        // Add DbSets for other models
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<UserFoodLog> UserFoodLogs { get; set; }
    }
}



