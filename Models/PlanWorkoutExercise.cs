using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepRecApi.Models;

public class PlanWorkoutExercise
{
    [Key] // PK with Auto-increment
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public required string UserId { get; set; }
    public required int PlanWorkoutId { get; set; }
    public required int OptExerciseCategoryId { get; set; }
    public required int OptExerciseId { get; set; }
    public required int DayIndex { get; set; }
    public required int DayOrder { get; set; }
    public required string ExerciseDefinitionJson { get; set; } // Dynamic JSON

    // Navigation properties / Foreign Key connections
    public User? User { get; set; }
    public required PlanWorkout PlanWorkout { get; set; }
    public required OptExerciseCategory OptExerciseCategory { get; set; }
    public required OptExercise OptExercise { get; set; }

}
