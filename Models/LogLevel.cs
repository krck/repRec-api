using System.ComponentModel.DataAnnotations;

namespace RepRecApi.Models;

public class LogLevel
{
    [Key]
    public required int Id { get; set; }

    public required string Name { get; set; }

    // Navigation properties / Foreign Key connections
    public ICollection<Log>? Logs { get; set; }
}
