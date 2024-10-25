using MacroTracker.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace MacroTracker.ViewModels
{
    public class EditFoodLogViewModel
    {
        public int UserFoodLogId { get; set; }
        public int FoodItemId { get; set; }  // Selected Food Item
        public double ServingSize { get; set; }  // Serving size logged
        public string MealType { get; set; }
        public double? Calories { get; set; }
        public double? Protein { get; set; }
        public double? Carbohydrates { get; set; }
        public double? Fat { get; set; }
        public double? Fiber { get; set; }
        public double? AddedSugar { get; set; }

        [BindNever]
        public FoodItem FoodItem { get; set; }
    }
}



