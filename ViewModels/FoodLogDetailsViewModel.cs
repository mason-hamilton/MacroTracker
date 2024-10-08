using MacroTracker.Models; // Assuming FoodItem is in this namespace

namespace MacroTracker.ViewModels
{
    public class FoodLogDetailsViewModel
    {
        // !! Purpose: Used for the food details view where users can adjust serving size. !!

        // The ID of the food item being logged
        public int FoodItemId { get; set; }

        // The name of the food item (e.g., "Chicken Breast")
        public string Name { get; set; }

        // The default serving size of the food (e.g., "100 grams")
        public double? DefaultServingSize { get; set; }

        // The unit of the serving size (e.g., "grams", "oz")
        public string ServingUnit { get; set; }

        // Calories per default serving
        public double? CaloriesPerServing { get; set; }

        // Macros per default serving
        public double? ProteinPerServing { get; set; }
        public double? CarbohydratesPerServing { get; set; }
        public double? FatPerServing { get; set; }

        // The serving size entered by the user
        public double? UserServingSize { get; set; }

        // Calculated macros and calories based on user's serving size
        public double? TotalCalories => (CaloriesPerServing * (UserServingSize / DefaultServingSize));
        public double? TotalProtein => ProteinPerServing * (UserServingSize / DefaultServingSize);
        public double? TotalCarbohydrates => CarbohydratesPerServing * (UserServingSize / DefaultServingSize);
        public double? TotalFat => FatPerServing * (UserServingSize / DefaultServingSize);

        // Constructor to populate the view model from a FoodItem
        public FoodLogDetailsViewModel(FoodItem foodItem)
        {
            FoodItemId = foodItem.FoodItemId;
            Name = foodItem.Name;
            DefaultServingSize = foodItem.ServingSize;
            ServingUnit = foodItem.ServingUnit;
            CaloriesPerServing = foodItem.CaloriesPerServing;
            ProteinPerServing = foodItem.Protein;
            CarbohydratesPerServing = foodItem.Carbohydrates;
            FatPerServing = foodItem.Fat;

            // Default the user's serving size to the default value initially
            UserServingSize = DefaultServingSize;
        }
    }
}
