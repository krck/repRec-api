using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepRecApi.Database.Migrations
{
    /// <inheritdoc />
    public partial class UserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SettingDistanceUnit",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SettingTimezone",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SettingWeightUnit",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsShared",
                table: "PlanWorkouts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SettingDistanceUnit",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SettingTimezone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SettingWeightUnit",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsShared",
                table: "PlanWorkouts");
        }
    }
}
