using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacroTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddNutrientColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AddedSugar",
                table: "UserFoodLogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Calories",
                table: "UserFoodLogs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Carbs",
                table: "UserFoodLogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Fats",
                table: "UserFoodLogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Fiber",
                table: "UserFoodLogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Protein",
                table: "UserFoodLogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedSugar",
                table: "UserFoodLogs");

            migrationBuilder.DropColumn(
                name: "Calories",
                table: "UserFoodLogs");

            migrationBuilder.DropColumn(
                name: "Carbs",
                table: "UserFoodLogs");

            migrationBuilder.DropColumn(
                name: "Fats",
                table: "UserFoodLogs");

            migrationBuilder.DropColumn(
                name: "Fiber",
                table: "UserFoodLogs");

            migrationBuilder.DropColumn(
                name: "Protein",
                table: "UserFoodLogs");
        }
    }
}
