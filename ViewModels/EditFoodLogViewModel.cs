using MacroTracker.Models;

namespace MacroTracker.ViewModels
{
    public class EditFoodLogViewModel
    {
        public int UserFoodLogId { get; set; }
        public int FoodItemId { get; set; }  // Selected Food Item
        public double ServingSize { get; set; }  // Serving size logged

        // List of all available food items for the dropdown
        public List<FoodItem> FoodItems { get; set; }
    }
}



