namespace RepRecApi.Models;

public class User
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required DateTime CreatedAt { get; set; }

    public ICollection<UserRole>? UserRoles { get; set; } // Navigation property
}
