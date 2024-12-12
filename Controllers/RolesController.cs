using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RepRecApi.Database;
using RepRecApi.Models;

namespace RepRecApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly RepRecDbContext _context;
    private readonly ILogger<PlanController> _logger;

    public RolesController(RepRecDbContext context, ILogger<PlanController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/roles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
    {
        return await _context.Roles.ToListAsync();
    }

    // GET: api/roles/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Role>> GetRole(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        return (role == null)
                ? NotFound()
                : role;
    }

}
