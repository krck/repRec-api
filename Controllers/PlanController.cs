using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepRecApi.Common.Attributes;
using RepRecApi.Common.Enums;
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
    [RoleAccess(EnumRoles.Admin, EnumRoles.Planner)]
    public ActionResult<IEnumerable<Plan>> Get()
    {
        try
        {
            var res = PlanNames.Select((wd, idx) => new Plan
            {
                Id = new Random().Next(1000, 9999),
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(idx)),
                Name = wd
            })
            .ToArray();

            return Ok(res);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex);
        }
    }
}
