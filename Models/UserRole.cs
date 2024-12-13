namespace RepRecApi.Models;

public class UserRole
{
    public required string UserId { get; set; }
    public required User User { get; set; }

    public int RoleId { get; set; }
    public required Role Role { get; set; }
}
