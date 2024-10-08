using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MacroTracker.Models;
using MacroTracker.ViewModels;

namespace MacroTracker.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly MacroTrackerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileController(MacroTrackerContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserProfile (Home Screen for Standard Users)
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Get the current logged-in user
            var currentUser = await _userManager.GetUserAsync(User);

            // Retrieve the current user's profile
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserId == currentUser.Id);

            if (userProfile == null)
            {
                // Redirect to profile creation if user doesn't have a profile
                return RedirectToAction("Create");
            }

            // Create the view model and populate it
            var userProfileVM = new UserProfileViewModel
            {
                UserProfileId = userProfile.UserProfileId,
               // UserId = userProfile.UserId,
                WeightInPounds = userProfile.WeightInPounds,
                Age = userProfile.Age,
                Gender = userProfile.Gender,
                ActivityLevel = userProfile.ActivityLevel,
                BodyFatPercentage = userProfile.BodyFatPercentage,

                // Convert height from meters to feet and inches
                HeightFeet = Math.Floor(userProfile.Height / 0.3048),  // Convert meters to feet
                HeightInches = (userProfile.Height / 0.0254) % 12       // Convert remaining height to inches
            };

            // Calculate daily calorie target using the Katch-McArdle formula
            double bmr = 370 + (21.6 * (userProfile.WeightInPounds / 2.20462) * (1 - userProfile.BodyFatPercentage / 100));
            double calorieTarget = bmr * GetActivityMultiplier(userProfile.ActivityLevel);

            // Pass user profile and calorie target to the view
            ViewData["CalorieTarget"] = calorieTarget;
            return View(userProfileVM);  // This view is the user's personalized home screen
        }


        // Helper method to get activity level multiplier
        private double GetActivityMultiplier(string activityLevel)
        {
            switch (activityLevel.ToLower())
            {
                case "sedentary":
                    return 1.2;
                case "light":
                    return 1.375;
                case "moderate":
                    return 1.55;
                case "active":
                    return 1.725;
                case "very active":
                    return 1.9;
                default:
                    return 1.2;
            }
        }

        // GET: UserProfile/Create (Profile Creation)
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        //POST: UserProfile/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserProfileViewModel userProfileVM)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                // Handle case where the user is not logged in
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Create a UserProfile object and assign the values
                var userProfile = new UserProfile
                {
                    UserId = currentUser.Id,
                    WeightInPounds = userProfileVM.WeightInPounds,
                    Age = userProfileVM.Age,
                    Gender = userProfileVM.Gender,
                    ActivityLevel = userProfileVM.ActivityLevel,
                    BodyFatPercentage = userProfileVM.BodyFatPercentage,

                    // Convert height from feet/inches to meters here
                    Height = userProfileVM.GetHeightInMeters()
                };

                _context.Add(userProfile);
                await _context.SaveChangesAsync();

                // Redirect to the home page after creating the profile
                return RedirectToAction("Index", "UserProfile");
            }

            return View(userProfileVM);
        }




        // GET: UserProfile/Edit (Profile Editing)
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserId == currentUser.Id);
            if (userProfile == null)
            {
                return NotFound();
            }

            // Populate the ViewModel with feet and inches
            var userProfileVM = new UserProfileViewModel
            {
                UserProfileId = userProfile.UserProfileId,
              //  UserId = userProfile.UserId,
                WeightInPounds = userProfile.WeightInPounds,
                Age = userProfile.Age,
                Gender = userProfile.Gender,
                ActivityLevel = userProfile.ActivityLevel,
                BodyFatPercentage = userProfile.BodyFatPercentage,

                // Convert height from meters to feet and inches
                HeightFeet = Math.Floor(userProfile.Height / 0.3048),  // Convert meters to feet
                HeightInches = (userProfile.Height / 0.0254) % 12       // Convert remaining height to inches
            };

            return View(userProfileVM);
        }


        // POST: UserProfile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserProfileViewModel userProfileVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserId == currentUser.Id);

                    if (userProfile == null)
                    {
                        return NotFound();
                    }

                    // Update the UserProfile object with the values from the ViewModel
                    userProfile.WeightInPounds = userProfileVM.WeightInPounds;
                    userProfile.Age = userProfileVM.Age;
                    userProfile.Gender = userProfileVM.Gender;
                    userProfile.ActivityLevel = userProfileVM.ActivityLevel;
                    userProfile.BodyFatPercentage = userProfileVM.BodyFatPercentage;

                    // Convert feet/inches to meters before saving
                    userProfile.Height = userProfileVM.GetHeightInMeters();

                    _context.Update(userProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProfileExists(userProfileVM.UserProfileId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userProfileVM);
        }


        // Admin Action: GET: UserProfile/ManageUsers (Admin-Only Page for Viewing All User Profiles)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUsers()
        {
            var allUsers = _context.UserProfiles.Include(u => u.User);
            return View(await allUsers.ToListAsync());
        }

        // Helper Method to check if UserProfile exists
        private bool UserProfileExists(int id)
        {
            return _context.UserProfiles.Any(e => e.UserProfileId == id);
        }
    }
}

