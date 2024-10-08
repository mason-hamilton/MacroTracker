using MacroTracker.Models;
namespace MacroTracker.ViewModels
{
    public class HomeIndexViewModel
    {
        public UserProfile UserProfile { get; set; }  // Holds daily calorie and macro targets
        public List<UserFoodLog> UserFoodLogs { get; set; }  // Holds today's logged foods

        // New property to hold available food items
        public List<FoodItem> AvailableFoodItems { get; set; }  // Holds the list of food items to select from
    }
}


