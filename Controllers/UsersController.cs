using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepRecApi.Database;
using RepRecApi.Models;

namespace RepRecApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly RepRecDbContext _context;
    private readonly ILogger<PlanController> _logger;

    public UsersController(RepRecDbContext context, ILogger<PlanController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return (user == null)
                ? NotFound()
                : user;
    }

    // Set User information (Name, Email)
    // This is called on every new login to initially create or update (keep up-to-date) the user information
    // POST: api/users
    [HttpPost("{id}")]
    public async Task<ActionResult<User>> SetUser(string id, [FromBody] User user)
    {
        if (user == null)
            return BadRequest("Invalid data.");

        var existingUser = await _context.Users.FindAsync(user.Id);
        if (existingUser == null)
        {
            _context.Users.Add(user);
        }
        else
        {
            existingUser.Email = user.Email;
            existingUser.EmailVerified = user.EmailVerified;
            existingUser.Nickname = user.Nickname;
        }
        await _context.SaveChangesAsync();

        return Ok(new { message = "Data received successfully", data = user });
    }

}
