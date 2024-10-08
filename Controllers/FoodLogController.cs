using MacroTracker.Models;
using MacroTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public IActionResult SearchFoodLog(string searchQuery = null)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
            SearchResults = searchResults
        };

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
            ModelState.AddModelError("FoodItemId", "Invalid food item selected.");
            return View(viewModel);
        }

        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var userFoodLog = new UserFoodLog
        {
            UserId = userId,
            FoodItemId = viewModel.SelectedFoodItemId, // Get the selected food item
            ServingSize = viewModel.ServingSize, // Get the entered serving size
            LogDate = DateTime.Now.Date // Log for today
        };

        _context.UserFoodLogs.Add(userFoodLog);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home"); // Go back to the home page
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

        _context.UserFoodLogs.Add(foodLog);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

}




