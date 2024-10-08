namespace MacroTracker.Models
{
    public class FoodItem
    {
        public int FoodItemId { get; set; }  // Primary key
        public string Name { get; set; }
        public double? CaloriesPerServing { get; set; }  // Calories per serving
        public double ServingSize { get; set; }  // Standard serving size for this item
        public string ServingUnit { get; set; }  // Unit of the serving size (e.g., grams, oz, ml)

        // Optional nutritional info
        public double? Protein { get; set; }  // Nullable, in grams
        public double? Carbohydrates { get; set; }  // Nullable, in grams
        public double? Fat { get; set; }  // Nullable, in grams
        public double? Fiber { get; set; }  // Nullable, in grams
        public double? AddedSugar { get; set; }  // Nullable, in grams
    }
}




