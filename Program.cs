using FluentAssertions.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the DbContext and use the connection string from appsettings.json
builder.Services.AddDbContext<MacroTracker.Models.MacroTrackerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity services
builder.Services.AddIdentity<MacroTracker.Models.ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MacroTracker.Models.MacroTrackerContext>()
    .AddDefaultTokenProviders();

// Register CORS policy for React frontend access
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000")  // URL of your React app
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("ReactPolicy"); // Enable CORS for the specified policy

app.UseAuthentication(); // Ensure authentication is used
app.UseAuthorization();

app.MapControllerRoute(
    name: "editFoodLog",
    pattern: "FoodLog/EditFoodLog/{id}",
    defaults: new { controller = "FoodLog", action = "EditFoodLog" });


app.MapControllerRoute(
    name: "foodlog",
    pattern: "FoodLog/{action}/{meal}",
    defaults: new { controller = "FoodLog", action = "SearchFoodLog" });

// New route specifically for delete actions
app.MapControllerRoute(
    name: "deleteFoodLog",
    pattern: "FoodLog/Delete/{id}",
    defaults: new { controller = "FoodLog", action = "DeleteFoodLog" });



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();


