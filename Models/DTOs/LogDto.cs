
namespace RepRecApi.Models.DTOs;

public class LogDto
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public required string Level { get; set; }
    public string? UserName { get; set; }
    public required string ExceptionType { get; set; }
    public string? Message { get; set; }
    public string? Source { get; set; }
}
