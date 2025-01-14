using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RepRecApi.Database.Migrations
{
    /// <inheritdoc />
    public partial class PlanningTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanWorkouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanWorkouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanWorkoutExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlanWorkoutId = table.Column<int>(type: "integer", nullable: false),
                    OptExerciseCategoryId = table.Column<int>(type: "integer", nullable: false),
                    OptExerciseId = table.Column<int>(type: "integer", nullable: false),
                    DayIndex = table.Column<int>(type: "integer", nullable: false),
                    DayOrder = table.Column<int>(type: "integer", nullable: false),
                    ExerciseDefinitionJson = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanWorkoutExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanWorkoutExercises_OptExerciseCategories_OptExerciseCateg~",
                        column: x => x.OptExerciseCategoryId,
                        principalTable: "OptExerciseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanWorkoutExercises_OptExercises_OptExerciseId",
                        column: x => x.OptExerciseId,
                        principalTable: "OptExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanWorkoutExercises_PlanWorkouts_PlanWorkoutId",
                        column: x => x.PlanWorkoutId,
                        principalTable: "PlanWorkouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanWorkoutExercises_OptExerciseCategoryId",
                table: "PlanWorkoutExercises",
                column: "OptExerciseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanWorkoutExercises_OptExerciseId",
                table: "PlanWorkoutExercises",
                column: "OptExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanWorkoutExercises_PlanWorkoutId",
                table: "PlanWorkoutExercises",
                column: "PlanWorkoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanWorkoutExercises");

            migrationBuilder.DropTable(
                name: "PlanWorkouts");
        }
    }
}
