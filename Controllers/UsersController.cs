using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RepRecApi.Common.Enums;
using RepRecApi.Database;
using RepRecApi.Models;

namespace RepRecApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly RepRecDbContext _context;

    public UsersController(RepRecDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GET: api/users/{id}
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        var dbUser = await _context.Users.Include(u => u.UserRoles).FirstAsync(u => u.Id == id);
        return (dbUser == null) ? NotFound() : Ok(dbUser);
    }

    /// <summary>
    /// POST: api/users
    /// 
    /// Set User information (Name, Email)
    /// This is called on every new login to initially create or update (keep up-to-date) the user information
    /// </summary>
    [HttpPost("{id}")]
    [Authorize]
    public async Task<ActionResult<User>> SetUser([FromBody] User user)
    {
        if (user == null)
            throw new ValidationException("User object is required");

        // Try to find a user and get all the assigned roles as well
        var dbUser = await _context.Users.Include(u => u.UserRoles).FirstAsync(u => u.Id == user.Id);
        if (dbUser == null)
        {
            // In case User does not already exist: Create new with the default role of "User"
            dbUser = _context.Users.Add(new User
            {
                Id = user.Id,
                Email = user.Email,
                EmailVerified = user.EmailVerified,
                Nickname = user.Nickname,
                CreatedAt = DateTime.Now.Date.ToUniversalTime(),
                UserRoles = new List<UserRole> {
                    new UserRole { UserId = user.Id, RoleId = (int)EnumRoles.User },
                    new UserRole { UserId = user.Id, RoleId = (int)EnumRoles.Planner }
                }
            }).Entity;

            await _context.SaveChangesAsync();
        }

        return Ok(dbUser);
    }

}
