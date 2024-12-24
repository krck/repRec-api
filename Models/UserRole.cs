namespace RepRecApi.Models;

public class UserRole
{
    public required string UserId { get; set; }
    public required int RoleId { get; set; }

    // Navigation properties / Foreign Key connections
    public User? User { get; set; }
    public Role? Role { get; set; }
}
