using System.Collections.Generic;

namespace MacroTracker.Models
{
    public class UserProfile
    {
        public int UserProfileId { get; set; }
        public string UserId { get; set; }

        // Store height in meters
        public double Height { get; set; }

        public double WeightInPounds { get; set; }  // Weight in pounds
        public int Age { get; set; }
        public string Gender { get; set; }
        public string ActivityLevel { get; set; }
        public double BodyFatPercentage { get; set; }  // Body fat percentage

        // Relationship with ApplicationUser
        public ApplicationUser User { get; set; }

        // New relationship with UserFoodLog
        public List<UserFoodLog> UserFoodLogs { get; set; } = new List<UserFoodLog>();

        // Protein target: 1 gram per pound of body weight
        public double ProteinTarget => WeightInPounds;

        // Fiber target: 38g for men, 25g for women
        public double FiberTarget => Gender.ToLower() == "male" ? 38 : 25;

        // Calculate the calorie target using Katch-McArdle formula
        public double CalculateCalorieTarget()
        {
            // Convert weight from pounds to kilograms
            double weightInKg = WeightInPounds * 0.453592;  // 1 pound = 0.453592 kg

            // Calculate lean body mass
            double leanBodyMass = weightInKg * (1 - (BodyFatPercentage / 100));

            // Calculate BMR using Katch-McArdle formula
            double bmr = 370 + (21.6 * leanBodyMass);

            // Calculate TDEE based on activity level
            double activityMultiplier = GetActivityMultiplier(ActivityLevel);

            return bmr * activityMultiplier;  // Return the calorie target
        }

        // Fat target: 20% of calorie target divided by 9 (calories per gram of fat)
        public double FatTarget => (CalculateCalorieTarget() * 0.20) / 9;

        // Carb target: Remaining calories after accounting for protein and fat, divided by 4 (calories per gram of carbs)
        public double CarbTarget
        {
            get
            {
                double calorieTarget = CalculateCalorieTarget();
                double proteinCals = ProteinTarget * 4;
                double fatCals = FatTarget * 9;
                double proAndFatCals = proteinCals + fatCals;
                return (calorieTarget - proAndFatCals) / 4;
            }
        }

        // Added sugar target: 10% of total calories divided by 4 (calories per gram of sugar)
        public double AddedSugarTarget
        {
            get
            {
                double tenPercentOfCals = CalculateCalorieTarget() * 0.10;
                return tenPercentOfCals / 4;
            }
        }

        // Get the activity multiplier based on the user's activity level
        private double GetActivityMultiplier(string activityLevel)
        {
            switch (activityLevel.ToLower())
            {
                case "sedentary":
                    return 1.2;
                case "lightly active":
                    return 1.375;
                case "moderately active":
                    return 1.55;
                case "very active":
                    return 1.725;
                case "super active":
                    return 1.9;
                default:
                    return 1.2;  // Default to sedentary
            }
        }
    }
}





