using Microsoft.AspNetCore.Authorization;
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

    public RolesController(RepRecDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GET: api/roles
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
    {
        return await _context.Roles.ToListAsync();
    }

    /// <summary>
    /// GET: api/roles/{id}
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Role>> GetRole(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        return (role == null)
                ? NotFound()
                : role;
    }

}
