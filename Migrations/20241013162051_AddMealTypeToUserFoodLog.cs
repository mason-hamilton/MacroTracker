using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacroTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddMealTypeToUserFoodLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MealType",
                table: "UserFoodLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MealType",
                table: "UserFoodLogs");
        }
    }
}
