using System.ComponentModel.DataAnnotations;

namespace RepRecApi.Models;

public class User
{
    [Key]
    public required string Id { get; set; } // This is the Auth0 user_id

    public required string Email { get; set; }
    public bool EmailVerified { get; set; }
    public string? Nickname { get; set; }
    public required string SettingTimezone { get; set; }
    public required string SettingWeightUnit { get; set; }
    public required string SettingDistanceUnit { get; set; }
    public required DateTime CreatedAt { get; set; }

    // Navigation properties / Foreign Key connections
    public ICollection<UserRole>? UserRoles { get; set; }
    public ICollection<PlanWorkout>? PlanWorkouts { get; set; }
    public ICollection<PlanWorkoutExercise>? PlanWorkoutExercises { get; set; }

}
