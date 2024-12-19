using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RepRecApi.Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using RepRecApi.Common.Enums;
using RepRecApi.Database;
using RepRecApi.Models;
using RepRecApi.Common;

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
    /// GET: api/users
    /// 
    /// Get all Users with their assigned Roles
    /// (This is only available to Admins)
    /// </summary>
    [HttpGet]
    [Authorize]
    [RoleAccess(EnumRoles.Admin)]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.Include(u => u.UserRoles).ToListAsync();
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

        // Access the HttpContext to check the User ID, based on the JWT Auth0 Token/Claims
        // !!! EVERYONE CAN ACCESS THIS ENDPOINT BUT USERS CAN ONLY CHANGE THEIR OWN DATA !!!
        string? httpUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == GlobalStaticVariables.Auth0UserIdClaim)?.Value;
        if (httpUserId == null || httpUserId != user.Id)
            throw new ValidationException("User object is invalid");

        // Try to find a user and get all the assigned roles as well
        var dbUser = await _context.Users.Include(u => u.UserRoles).FirstAsync(u => u.Id == user.Id);

        // In case User does not already exist: Create new with the default role of "User"
        if (dbUser == null)
        {
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

        // In case the User exists, but any data has changed: Update the User
        if (dbUser.Email != user.Email || dbUser.Nickname != user.Nickname || !dbUser.EmailVerified)
        {
            dbUser.Email = user.Email;
            dbUser.EmailVerified = user.EmailVerified;
            dbUser.Nickname = user.Nickname;
            await _context.SaveChangesAsync();
        }

        return Ok(dbUser);
    }

}
