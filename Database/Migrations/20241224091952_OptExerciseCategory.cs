using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RepRecApi.Database.Migrations
{
    /// <inheritdoc />
    public partial class OptExerciseCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "OptExercises");

            migrationBuilder.AlterColumn<string>(
                name: "Mechanic",
                table: "OptExercises",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "OptExercises",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OptExerciseCategoryId",
                table: "OptExercises",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OptExerciseCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    JsonTemplate = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptExerciseCategories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "OptExerciseCategories",
                columns: new[] { "Id", "Description", "JsonTemplate", "Name" },
                values: new object[,]
                {
                    { 1, "Bodybuilding and Power-Lifting exercises", "[]", "Weightlifting" },
                    { 2, "Snatch, Clean & Jerk, etc.", "[]", "Olympic Lifting" },
                    { 3, "Atlas Stones, Tire Flips, etc.", "[]", "Strongman" },
                    { 4, "Box Jumps, Jump Squats, etc.", "[]", "Plyometrics" },
                    { 5, "Static, Dynamic, PNF, etc.", "[]", "Stretching" },
                    { 6, "All Forms of Cardio", "[]", "Endurance Training" },
                    { 7, "Yoga, Pilates, Calisthenics, Courses, etc.", "[]", "Physical Exercises" },
                    { 8, "Hiking, Swimming, Bouldering, Outdoor, etc.", "[]", "Other Activities" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OptExercises_OptExerciseCategoryId",
                table: "OptExercises",
                column: "OptExerciseCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_OptExercises_OptExerciseCategories_OptExerciseCategoryId",
                table: "OptExercises",
                column: "OptExerciseCategoryId",
                principalTable: "OptExerciseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OptExercises_OptExerciseCategories_OptExerciseCategoryId",
                table: "OptExercises");

            migrationBuilder.DropTable(
                name: "OptExerciseCategories");

            migrationBuilder.DropIndex(
                name: "IX_OptExercises_OptExerciseCategoryId",
                table: "OptExercises");

            migrationBuilder.DropColumn(
                name: "OptExerciseCategoryId",
                table: "OptExercises");

            migrationBuilder.AlterColumn<string>(
                name: "Mechanic",
                table: "OptExercises",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "OptExercises",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "OptExercises",
                type: "text",
                nullable: true);
        }
    }
}
