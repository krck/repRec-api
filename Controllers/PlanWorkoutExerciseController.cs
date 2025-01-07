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
public class PlanWorkoutExerciseController : ControllerBase
{
    private readonly RepRecDbContext _context;

    public PlanWorkoutExerciseController(RepRecDbContext context)
    {
        _context = context;
    }

    #region QUERY

    /// <summary>
    /// GET: api/PlanWorkoutExercise/5
    /// (by PlanWorkout)
    /// </summary>
    [HttpGet("{planWorkoutId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<PlanWorkoutExercise>>> GetPlanWorkoutExercises(int planWorkoutId)
    {
        // Access the HttpContext to check the User ID, based on the JWT Auth0 Token/Claims
        string? httpUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == GlobalStaticVariables.Auth0UserIdClaim)?.Value;
        if (string.IsNullOrWhiteSpace(httpUserId))
            throw new ValidationException("User is invalid");

        // Users can only see their own PlanWorkoutExercises
        var allPlanWorkoutExercises = await _context.PlanWorkoutExercises
            .Where(pw => pw.UserId == httpUserId && pw.PlanWorkoutId == planWorkoutId)
            .ToListAsync();

        return Ok(allPlanWorkoutExercises);
    }

    #endregion QUERY

    #region MUTATION

    /// <summary>
    /// PUT: api/PlanWorkoutExercise/5
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    [RoleAccess(EnumRoles.Admin)]
    public async Task<ActionResult<PlanWorkoutExercise>> PutPlanWorkoutExercise(int id, PlanWorkoutExercise planWorkoutExercise)
    {
        if (id != planWorkoutExercise.Id)
            throw new ValidationException("Invalid PlanWorkoutExercise Id");

        // Access the HttpContext to check the User ID, based on the JWT Auth0 Token/Claims
        string? httpUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == GlobalStaticVariables.Auth0UserIdClaim)?.Value;
        if (string.IsNullOrWhiteSpace(httpUserId) || planWorkoutExercise.UserId != httpUserId)
            throw new ValidationException("User is invalid");

        // If its a valid Entity, let EF handle the update automatically
        _context.Entry(planWorkoutExercise).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(planWorkoutExercise);
    }

    /// <summary>
    /// POST: api/PlanWorkoutExercise
    /// </summary>
    [HttpPost]
    [Authorize]
    [RoleAccess(EnumRoles.Admin)]
    public async Task<ActionResult<PlanWorkoutExercise>> PostPlanWorkoutExercise(PlanWorkoutExercise planWorkoutExercise)
    {
        // Access the HttpContext to check the User ID, based on the JWT Auth0 Token/Claims
        string? httpUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == GlobalStaticVariables.Auth0UserIdClaim)?.Value;
        if (string.IsNullOrWhiteSpace(httpUserId) || planWorkoutExercise.UserId != httpUserId)
            throw new ValidationException("User is invalid");

        // Always overwrite the UserId
        planWorkoutExercise.UserId = httpUserId;
        var dbItem = _context.PlanWorkoutExercises.Add(planWorkoutExercise);
        await _context.SaveChangesAsync();

        return Ok(dbItem.Entity);
    }

    /// <summary>
    /// DELETE: api/PlanWorkoutExercise/5
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    [RoleAccess(EnumRoles.Admin)]
    public async Task<ActionResult<PlanWorkoutExercise>> DeletePlanWorkoutExercise(int id)
    {
        var planWorkoutExercise = await _context.PlanWorkoutExercises.FindAsync(id);
        if (planWorkoutExercise == null)
            throw new ValidationException("Unknown PlanWorkoutExercise Id");

        // Access the HttpContext to check the User ID, based on the JWT Auth0 Token/Claims
        string? httpUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == GlobalStaticVariables.Auth0UserIdClaim)?.Value;
        if (string.IsNullOrWhiteSpace(httpUserId) || planWorkoutExercise.UserId != httpUserId)
            throw new ValidationException("User is invalid");

        _context.PlanWorkoutExercises.Remove(planWorkoutExercise);
        await _context.SaveChangesAsync();

        return Ok(planWorkoutExercise);
    }

    #endregion MUTATION
}
