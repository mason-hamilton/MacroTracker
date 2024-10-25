using MacroTracker.Models;

namespace MacroTracker.ViewModels
{
    public class AddFoodLogViewModel
    {
        // !! Purpose: Used for the food search functionality. !!

        // The search query entered by the user
        public string SearchQuery { get; set; }

        // The list of search results (i.e., food items matching the search)
        public List<FoodItem> SearchResults { get; set; }

        // The food item ID the user selects to log
        public int SelectedFoodItemId { get; set; }

        // The serving size entered by the user
        public double ServingSize { get; set; }

        // New Property to track which meal the food log is associated with
        public string MealType { get; set; }

        // Additional properties can be added if needed (e.g., for UI purposes)
    }
}

