using System.ComponentModel.DataAnnotations;

namespace RepRecApi.Models;

public class Role
{
    [Key]
    public int Id { get; set; }

    public required string Name { get; set; }

    // Navigation properties / Foreign Key connections
    public ICollection<UserRole>? UserRoles { get; set; }
}
