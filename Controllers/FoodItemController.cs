using MacroTracker.Models;
using Microsoft.AspNetCore.Mvc;

public class FoodItemController : Controller
{
    private readonly MacroTrackerContext _context;

    public FoodItemController(MacroTrackerContext context)
    {
        _context = context;
    }

    // Displays the form for creating a new food item
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Handles the form submission for creating a new food item
    [HttpPost]
    public IActionResult Create(FoodItem model)
    {
        if (ModelState.IsValid)
        {
            _context.FoodItems.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }
}

