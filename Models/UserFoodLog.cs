using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MacroTracker.Models
{
    public class UserFoodLog
    {
        public int UserFoodLogId { get; set; }  // Primary key
        public string UserId { get; set; }  // Foreign key to the ApplicationUser table

        [ForeignKey("FoodItemId")]
        public int FoodItemId { get; set; }  // Foreign key to the FoodItem table
        public DateTime LogDate { get; set; }  // Date of food entry
        public double ServingSize { get; set; }  // How much of the food was consumed (actual serving size)

        // Calculated properties based on the user's logged serving size
        public double? Calories => (ServingSize / FoodItem.ServingSize) * FoodItem.CaloriesPerServing;
        public double Protein => (ServingSize / FoodItem.ServingSize) * FoodItem.Protein.GetValueOrDefault();
        public double Carbs => (ServingSize / FoodItem.ServingSize) * FoodItem.Carbohydrates.GetValueOrDefault();
        public double Fats => (ServingSize / FoodItem.ServingSize) * FoodItem.Fat.GetValueOrDefault();
        public double Fiber => (ServingSize / FoodItem.ServingSize) * FoodItem.Fiber.GetValueOrDefault();
        public double AddedSugar => (ServingSize / FoodItem.ServingSize) * FoodItem.AddedSugar.GetValueOrDefault();

        // Relationships
        public ApplicationUser User { get; set; }  // Reference to the user
        public FoodItem FoodItem { get; set; }  // Reference to the food item
    }
}





