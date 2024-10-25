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

        // New Foreign Key for UserProfile
        [ForeignKey("UserProfile")]
        public int? UserProfileId { get; set; }  // Foreign key to the UserProfile table

        // Change calculated properties to regular properties
        public double? Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }
        public double Fiber { get; set; }
        public double AddedSugar { get; set; }

        // Relationships
        public ApplicationUser User { get; set; }  // Reference to the user
        public FoodItem FoodItem { get; set; }  // Reference to the food item
        [Required]
        public string MealType { get; set; }  // Breakfast, Lunch, Dinner, Snacks

        public UserProfile UserProfile { get; set; }  // Reference to UserProfile

        // Method to update the nutrient values based on serving size and food item
        public void RecalculateNutrients()
        {
            if (FoodItem != null)
            {
                Calories = (ServingSize / FoodItem.ServingSize) * FoodItem.CaloriesPerServing;
                Protein = (ServingSize / FoodItem.ServingSize) * FoodItem.Protein.GetValueOrDefault();
                Carbs = (ServingSize / FoodItem.ServingSize) * FoodItem.Carbohydrates.GetValueOrDefault();
                Fats = (ServingSize / FoodItem.ServingSize) * FoodItem.Fat.GetValueOrDefault();
                Fiber = (ServingSize / FoodItem.ServingSize) * FoodItem.Fiber.GetValueOrDefault();
                AddedSugar = (ServingSize / FoodItem.ServingSize) * FoodItem.AddedSugar.GetValueOrDefault();
            }
        }
    }
}






