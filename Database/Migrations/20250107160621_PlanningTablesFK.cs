using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepRecApi.Database.Migrations
{
    /// <inheritdoc />
    public partial class PlanningTablesFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PlanWorkouts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PlanWorkoutExercises",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PlanWorkouts_UserId",
                table: "PlanWorkouts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanWorkoutExercises_UserId",
                table: "PlanWorkoutExercises",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanWorkoutExercises_Users_UserId",
                table: "PlanWorkoutExercises",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanWorkouts_Users_UserId",
                table: "PlanWorkouts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanWorkoutExercises_Users_UserId",
                table: "PlanWorkoutExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanWorkouts_Users_UserId",
                table: "PlanWorkouts");

            migrationBuilder.DropIndex(
                name: "IX_PlanWorkouts_UserId",
                table: "PlanWorkouts");

            migrationBuilder.DropIndex(
                name: "IX_PlanWorkoutExercises_UserId",
                table: "PlanWorkoutExercises");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PlanWorkouts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PlanWorkoutExercises");
        }
    }
}
