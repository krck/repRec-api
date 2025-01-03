using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepRecApi.Models;

public class Log
{
    [Key] // PK with Auto-increment
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }

    public required int LogLevelId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public required string ExceptionType { get; set; }
    public string? Message { get; set; }
    public string? StackTrace { get; set; }
    public string? Source { get; set; } // Optional: where the error occurred
    public string? userId { get; set; }

    // Navigation properties / Foreign Key connections
    public LogLevel? LogLevel { get; set; }
}
