using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepRecApi.Models;

public class OptExerciseCategory
{
    [Key] // PK with Auto-increment
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }

    public required string Name { get; set; }
    public required string JsonTemplate { get; set; }
    public string? Description { get; set; }

    // Navigation properties / Foreign Key connections
    public ICollection<OptExercise>? OptExercises { get; set; }
    public ICollection<PlanWorkoutExercise>? PlanWorkoutExercises { get; set; }

}
