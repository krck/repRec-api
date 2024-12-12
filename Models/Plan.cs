namespace RepRecApi.Models;

public class Plan
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateOnly Date { get; set; }
}
