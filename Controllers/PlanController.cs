using Microsoft.AspNetCore.Mvc;

namespace repRec_api.Controllers;

[ApiController]
[Route("[controller]")]
public class PlanController : ControllerBase
{
    private static readonly string[] WorkoutDays = new[]
    {
        "Leg Day", "Chest Day", "Back Day", "Shoulder Day", "Arm Day"
    };

    private readonly ILogger<PlanController> _logger;

    public PlanController(ILogger<PlanController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetPlan")]
    public IEnumerable<Plan> Get()
    {
        return WorkoutDays.Select(wd => new Plan
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(Random.Shared.Next(WorkoutDays.Length))),
            Workout = wd
        })
        .ToArray();
    }
}
