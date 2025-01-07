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
public class PlanWorkoutController : ControllerBase
{
    private readonly RepRecDbContext _context;

    public PlanWorkoutController(RepRecDbContext context)
    {
        _context = context;
    }

    #region QUERY

    /// <summary>
    /// GET: api/PlanWorkout
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<PlanWorkout>>> GetPlanWorkouts()
    {
        // Access the HttpContext to check the User ID, based on the JWT Auth0 Token/Claims
        string? httpUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == GlobalStaticVariables.Auth0UserIdClaim)?.Value;
        if (string.IsNullOrWhiteSpace(httpUserId))
            throw new ValidationException("User is invalid");

        // Users can only see their own PlanWorkouts
        var allPlanWorkouts = await _context.PlanWorkouts.Where(pw => pw.UserId == httpUserId).ToListAsync();
        return Ok(allPlanWorkouts);
    }

    /// <summary>
    /// GET: api/PlanWorkout/5
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<PlanWorkout>> GetPlanWorkout(int id)
    {
        // Access the HttpContext to check the User ID, based on the JWT Auth0 Token/Claims
        string? httpUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == GlobalStaticVariables.Auth0UserIdClaim)?.Value;
        if (string.IsNullOrWhiteSpace(httpUserId))
            throw new ValidationException("User is invalid");

        var planWorkout = await _context.PlanWorkouts.FirstOrDefaultAsync(x => x.Id == id && x.UserId == httpUserId);
        if (planWorkout == null)
            throw new ValidationException("Unknown PlanWorkout Id");

        return Ok(planWorkout);
    }

    #endregion QUERY

    #region MUTATION

    /// <summary>
    /// PUT: api/PlanWorkout/5
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    [RoleAccess(EnumRoles.Admin)]
    public async Task<ActionResult<PlanWorkout>> PutPlanWorkout(int id, PlanWorkout planWorkout)
    {
        if (id != planWorkout.Id)
            throw new ValidationException("Invalid PlanWorkout Id");

        // Access the HttpContext to check the User ID, based on the JWT Auth0 Token/Claims
        string? httpUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == GlobalStaticVariables.Auth0UserIdClaim)?.Value;
        if (string.IsNullOrWhiteSpace(httpUserId) || planWorkout.UserId != httpUserId)
            throw new ValidationException("User is invalid");

        // If its a valid Entity, let EF handle the update automatically
        _context.Entry(planWorkout).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(planWorkout);
    }

    /// <summary>
    /// POST: api/PlanWorkout
    /// </summary>
    [HttpPost]
    [Authorize]
    [RoleAccess(EnumRoles.Admin)]
    public async Task<ActionResult<PlanWorkout>> PostPlanWorkout(PlanWorkout planWorkout)
    {
        // Access the HttpContext to check the User ID, based on the JWT Auth0 Token/Claims
        string? httpUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == GlobalStaticVariables.Auth0UserIdClaim)?.Value;
        if (string.IsNullOrWhiteSpace(httpUserId))
            throw new ValidationException("User is invalid");

        // Always overwrite the UserId
        planWorkout.UserId = httpUserId;
        planWorkout.CreatedAt = DateTime.UtcNow;
        var dbItem = _context.PlanWorkouts.Add(planWorkout);
        await _context.SaveChangesAsync();

        return Ok(dbItem.Entity);
    }

    /// <summary>
    /// DELETE: api/PlanWorkout/5
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    [RoleAccess(EnumRoles.Admin)]
    public async Task<ActionResult<PlanWorkout>> DeletePlanWorkout(int id)
    {
        var planWorkout = await _context.PlanWorkouts.FindAsync(id);
        if (planWorkout == null)
            throw new ValidationException("Unknown PlanWorkout Id");

        // Access the HttpContext to check the User ID, based on the JWT Auth0 Token/Claims
        string? httpUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == GlobalStaticVariables.Auth0UserIdClaim)?.Value;
        if (string.IsNullOrWhiteSpace(httpUserId) || planWorkout.UserId != httpUserId)
            throw new ValidationException("User is invalid");

        _context.PlanWorkouts.Remove(planWorkout);
        await _context.SaveChangesAsync();

        return Ok(planWorkout);
    }

    #endregion MUTATION
}
