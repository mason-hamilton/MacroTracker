namespace MacroTracker.ViewModels
{
    public class UserProfileViewModel
    {
        public int UserProfileId { get; set; }
       // public string UserId { get; set; }

        // These are the fields you display for user input
        public double HeightFeet { get; set; }
        public double HeightInches { get; set; }
        public double WeightInPounds { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string ActivityLevel { get; set; }
        public double BodyFatPercentage { get; set; }

        // Use this method to convert feet/inches to meters
        public double GetHeightInMeters()
        {
            double totalInches = (HeightFeet * 12) + HeightInches;
            return totalInches * 0.0254; // Convert inches to meters
        }
    }
}


