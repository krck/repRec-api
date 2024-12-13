using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepRecApi.Models;

namespace RepRecApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlanController : ControllerBase
{
    private static readonly string[] PlanNames = new[]
    {
        "Push/Pull/Legs", "Upper/Lower", "Full Body", "Bro Split"
    };

    private readonly ILogger<PlanController> _logger;

    public PlanController(ILogger<PlanController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public IEnumerable<Plan> Get()
    {
        var res = PlanNames.Select((wd, idx) => new Plan
        {
            Id = new Random().Next(1000, 9999),
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(idx)),
            Name = wd
        })
        .ToArray();

        return res;
    }
}
