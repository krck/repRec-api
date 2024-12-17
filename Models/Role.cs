using System.ComponentModel.DataAnnotations;

namespace RepRecApi.Models;

public class Role
{
    [Key]
    public int Id { get; set; }

    public required string Name { get; set; }

    public ICollection<UserRole>? UserRoles { get; set; } // Navigation property
}
