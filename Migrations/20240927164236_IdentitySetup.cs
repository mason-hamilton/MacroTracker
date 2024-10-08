using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacroTracker.Migrations
{
    /// <inheritdoc />
    public partial class IdentitySetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "UserProfiles",
                newName: "WeightInPounds");

            migrationBuilder.AddColumn<double>(
                name: "BodyFatPercentage",
                table: "UserProfiles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "UserFoodLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServingUnit",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserFoodLogs_UserProfileId",
                table: "UserFoodLogs",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFoodLogs_UserProfiles_UserProfileId",
                table: "UserFoodLogs",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFoodLogs_UserProfiles_UserProfileId",
                table: "UserFoodLogs");

            migrationBuilder.DropIndex(
                name: "IX_UserFoodLogs_UserProfileId",
                table: "UserFoodLogs");

            migrationBuilder.DropColumn(
                name: "BodyFatPercentage",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "UserFoodLogs");

            migrationBuilder.DropColumn(
                name: "ServingUnit",
                table: "FoodItems");

            migrationBuilder.RenameColumn(
                name: "WeightInPounds",
                table: "UserProfiles",
                newName: "Weight");
        }
    }
}
