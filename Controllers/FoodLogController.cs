using MacroTracker.Models;
using MacroTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

[Authorize]
public class FoodLogController : Controller
{
    private readonly MacroTrackerContext _context;

    public FoodLogController(MacroTrackerContext context)
    {
        _context = context;
    }

    // Display the search bar and search results when the user wants to add a food log
    public IActionResult SearchFoodLog(string meal, string searchQuery = null)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        Debug.WriteLine("The param for meal in SEARCH FOOD LOG IS: " + meal);

        // Initialize an empty list of search results
        List<FoodItem> searchResults = new List<FoodItem>();

        // If a search query is provided, find matching food items
        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchResults = _context.FoodItems
                .Where(fi => fi.Name.ToLower().Contains(searchQuery.ToLower()))
                .ToList();
        }

        // Create the ViewModel and pass it to the view
        var viewModel = new AddFoodLogViewModel
        {
            SearchQuery = searchQuery,
            SearchResults = searchResults,
            MealType = meal
        };

        ViewData["MealType"] = meal; // Pass the meal type to the view
      
        return View("~/Views/FoodLog/SearchFoodLog.cshtml", viewModel);
    }

    // Handle the food log creation after the user selects a food item and enters the serving size
    [HttpPost]
    public IActionResult AddFoodLog(AddFoodLogViewModel viewModel)
    {
        // Ensure FoodItemId is valid
        var foodItem = _context.FoodItems.FirstOrDefault(fi => fi.FoodItemId == viewModel.SelectedFoodItemId);

        if (foodItem == null)
        {
            // Return a JSON response for an invalid food item
            return Json(new { success = false, message = "Unable to add food. The selected food item does not exist." });
        }

        // Get the user ID
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Find the associated UserProfile based on the current user
        var userProfile = _context.UserProfiles.FirstOrDefault(up => up.UserId == userId);

        if (userProfile == null)
        {
            return Json(new { success = false, message = "User profile not found." });
        }

        // Calculate the ratio of the logged serving size to the standard serving size
        double servingRatio = viewModel.ServingSize / foodItem.ServingSize;

        // Calculate the nutrient values based on the logged serving size
        var userFoodLog = new UserFoodLog
        {
            UserId = userId,
            FoodItemId = viewModel.SelectedFoodItemId,
            ServingSize = viewModel.ServingSize,
            LogDate = DateTime.Now.Date,
            MealType = viewModel.MealType,
            UserProfileId = userProfile.UserProfileId,

            // Calculate nutrients based on serving ratio
            Calories = foodItem.CaloriesPerServing * servingRatio,
            Protein = (foodItem.Protein ?? 0) * servingRatio,
            Carbs = (foodItem.Carbohydrates ?? 0) * servingRatio,
            Fats = (foodItem.Fat ?? 0) * servingRatio,
            Fiber = (foodItem.Fiber ?? 0) * servingRatio,
            AddedSugar = (foodItem.AddedSugar ?? 0) * servingRatio

        };

        // Recalculate nutrients for this food log entry
        userFoodLog.RecalculateNutrients();

        // Add the food log entry to the database
        _context.UserFoodLogs.Add(userFoodLog);
        _context.SaveChanges();

        // Return a JSON response for a successful addition
        return Json(new { success = true, message = "Food logged successfully." });
    }



    [HttpGet]
    public IActionResult Details(int id)
    {
        var foodItem = _context.FoodItems.Find(id);

        if (foodItem == null)
        {
            return NotFound();
        }

        var viewModel = new FoodLogDetailsViewModel(foodItem);

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult AddFoodLogDirectlyFromSearch(int foodItemId, double servingSize)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var foodLog = new UserFoodLog
        {
            UserId = userId,
            FoodItemId = foodItemId,
            ServingSize = servingSize,
            LogDate = DateTime.Now.Date
        };

        // Recalculate nutrients for this food log entry
        foodLog.RecalculateNutrients();

        _context.UserFoodLogs.Add(foodLog);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult DeleteFoodLog([FromRoute] int id)
    {
        Debug.WriteLine(id);
        var userFoodLog = _context.UserFoodLogs.FirstOrDefault(log => log.UserFoodLogId == id);
        Debug.WriteLine(id);
        if (userFoodLog == null)
        {
            Debug.WriteLine("UserFoodLog is null!!");
            return NotFound(); // Handle the case where the log isn't found
        }

        // Recalculate nutrients for this food log entry
        userFoodLog.RecalculateNutrients();

        // Remove the food log from the database
        _context.UserFoodLogs.Remove(userFoodLog);
        _context.SaveChanges();

        // Redirect back to the home page or where appropriate
        return RedirectToAction("Index", "Home");
    }

    // GET: FoodLog/Edit/5
    public IActionResult EditFoodLog(int id)
    {
        Debug.WriteLine(id);
        // Retrieve the user food log by id
        var userFoodLog = _context.UserFoodLogs
            .Include(u => u.FoodItem) // Include FoodItem details
            .FirstOrDefault(log => log.UserFoodLogId == id);

        if (userFoodLog == null)
        {
            Debug.WriteLine("UserFoodLog is NULL!");
            return NotFound();
        }

        var editViewModel = new EditFoodLogViewModel
        {
            UserFoodLogId = userFoodLog.UserFoodLogId,
            FoodItemId = userFoodLog.FoodItemId,
            ServingSize = userFoodLog.ServingSize,
            MealType = userFoodLog.MealType,
            // Map the food item details for the view
            Calories = userFoodLog.FoodItem.CaloriesPerServing * userFoodLog.ServingSize / userFoodLog.FoodItem.ServingSize,
            Protein = userFoodLog.FoodItem.Protein * userFoodLog.ServingSize / userFoodLog.FoodItem.ServingSize,
            Carbohydrates = userFoodLog.FoodItem.Carbohydrates * userFoodLog.ServingSize / userFoodLog.FoodItem.ServingSize,
            Fat = userFoodLog.FoodItem.Fat * userFoodLog.ServingSize / userFoodLog.FoodItem.ServingSize,
            Fiber = userFoodLog.FoodItem.Fiber * userFoodLog.ServingSize / userFoodLog.FoodItem.ServingSize,
            AddedSugar = userFoodLog.FoodItem.AddedSugar * userFoodLog.ServingSize / userFoodLog.FoodItem.ServingSize,
            FoodItem = userFoodLog.FoodItem  // Pass the entire FoodItem object if needed
        };

        return View("Details", editViewModel); // Reusing the Details view
    }

    // POST: FoodLog/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(EditFoodLogViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userFoodLog = _context.UserFoodLogs.Find(model.UserFoodLogId);
            if (userFoodLog != null)
            {
                userFoodLog.ServingSize = model.ServingSize;
                userFoodLog.MealType = model.MealType;
                // Optionally update FoodItemId if needed
                // userFoodLog.FoodItemId = model.FoodItemId;

                // Recalculate nutrients for this food log entry
                userFoodLog.RecalculateNutrients();

                _context.Update(userFoodLog);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home"); // Redirect after successful edit
            }
            return NotFound();
        }
        return View(model); // Return the model if validation fails
    }

    [HttpPost]
    public async Task<IActionResult> SaveEdit(EditFoodLogViewModel model)
    {
        
        ModelState.Remove("FoodItem"); // Ignore validation for FoodItem object - This line is VERY IMPORTANT!
        //^DO NOT remove as it will cuase issues with model validation even though FoodItem has a [BindNever] attribute within the viewmodel

        if (ModelState.IsValid)
        {
            var userFoodLog = await _context.UserFoodLogs
                .Include(ufl => ufl.FoodItem)  // Ensure that FoodItem is loaded
                .FirstOrDefaultAsync(ufl => ufl.UserFoodLogId == model.UserFoodLogId);

            if (userFoodLog == null)
            {
                return NotFound();
            }

            // Update serving size and meal type
            userFoodLog.ServingSize = model.ServingSize;
            userFoodLog.MealType = model.MealType;

            // Load the FoodItem based on the FoodItemId
            //userFoodLog.FoodItem = await _context.FoodItems.FirstOrDefaultAsync(fi => fi.FoodItemId == model.FoodItemId);
            if (userFoodLog.FoodItem == null)
            {
                ModelState.AddModelError("FoodItem", "The selected food item was not found.");
                return View("Details", model);
            }

            // Recalculate nutrient values
            userFoodLog.RecalculateNutrients();

            // Save changes to the database
            _context.Update(userFoodLog);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        // If ModelState is invalid, re-fetch the necessary data
        var foodLog = await _context.UserFoodLogs
            .Include(ufl => ufl.FoodItem)  // Ensure related FoodItem is loaded
            .FirstOrDefaultAsync(ufl => ufl.UserFoodLogId == model.UserFoodLogId);

        if (foodLog == null)
        {
            return NotFound();
        }

        // Populate missing data in the model before returning the view
        model.FoodItem = foodLog.FoodItem;
        model.Calories = foodLog.Calories;
        model.MealType = foodLog.MealType;
        model.ServingSize = foodLog.ServingSize;


        return View("Details", model);
    }











}




