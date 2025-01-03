using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepRecApi.Models;

public class OptExercise
{
    [Key] // PK with Auto-increment
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }

    public required string Name { get; set; }
    public required int OptExerciseCategoryId { get; set; }
    public required string Level { get; set; }
    public required string PrimaryMuscles { get; set; }     // Comma separated
    public string? SecondaryMuscles { get; set; }           // Comma separated
    public string? Mechanic { get; set; }
    public string? Force { get; set; }
    public string? Equipment { get; set; }
    public string? Instructions { get; set; }

    // Navigation properties / Foreign Key connections
    public OptExerciseCategory? OptExerciseCategory { get; set; }
}
