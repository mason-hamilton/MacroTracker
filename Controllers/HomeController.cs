using MacroTracker.Models;
using MacroTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize]
public class HomeController : Controller
{
    private readonly MacroTrackerContext _context;

    public HomeController(MacroTrackerContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var userProfile = _context.UserProfiles.FirstOrDefault(up => up.UserId == userId);
        if (userProfile == null)
        {
            return RedirectToAction("Create", "UserProfile");
        }

        var today = DateTime.Now.Date;
        var userFoodLogs = _context.UserFoodLogs
            .Include(uf => uf.FoodItem)
            .Where(uf => uf.UserId == userId && uf.LogDate == today)
            .ToList();

        // Fetch the available food items to populate the dropdown in the view
        var availableFoodItems = _context.FoodItems.ToList();

        var viewModel = new HomeIndexViewModel
        {
            UserProfile = userProfile,
            UserFoodLogs = userFoodLogs,
            AvailableFoodItems = availableFoodItems  // Pass the available food items to the view
        };

        return View(viewModel);
    }
}









