using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RepRecApi.Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using RepRecApi.Common.Enums;
using RepRecApi.Database;
using RepRecApi.Models;

namespace RepRecApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OptExerciseController : ControllerBase
{
    private readonly RepRecDbContext _context;

    public OptExerciseController(RepRecDbContext context)
    {
        _context = context;
    }

    #region QUERY

    /// <summary>
    /// GET: api/OptExercise1
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<OptExercise>>> GetOptExercises()
    {
        var allOptExercises = await _context.OptExercises.ToListAsync();
        return Ok(allOptExercises);
    }

    /// <summary>
    /// GET: api/OptExercise/5
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<OptExercise>> GetOptExercise(int id)
    {
        var optExercise = await _context.OptExercises.FindAsync(id);
        if (optExercise == null)
            throw new ValidationException("Unknown OptExercise Id");

        return Ok(optExercise);
    }

    #endregion QUERY

    #region MUTATION

    /// <summary>
    /// PUT: api/OptExercise/5
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    [RoleAccess(EnumRoles.Admin)]
    public async Task<ActionResult<OptExercise>> PutOptExercise(int id, OptExercise optExercise)
    {
        if (id != optExercise.Id)
            throw new ValidationException("Invalid OptExercise Id");

        // If its a valid Entity, let EF handle the update automatically
        _context.Entry(optExercise).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(optExercise);
    }

    /// <summary>
    /// POST: api/OptExercise
    /// </summary>
    [HttpPost]
    [Authorize]
    [RoleAccess(EnumRoles.Admin)]
    public async Task<ActionResult<OptExercise>> PostOptExercise(OptExercise optExercise)
    {
        var dbItem = _context.OptExercises.Add(optExercise);
        await _context.SaveChangesAsync();

        return Ok(dbItem.Entity);
    }

    /// <summary>
    /// DELETE: api/OptExercise/5
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    [RoleAccess(EnumRoles.Admin)]
    public async Task<ActionResult<OptExercise>> DeleteOptExercise(int id)
    {
        var optExercise = await _context.OptExercises.FindAsync(id);
        if (optExercise == null)
            throw new ValidationException("Unknown OptExercise Id");

        _context.OptExercises.Remove(optExercise);
        await _context.SaveChangesAsync();

        return Ok(optExercise);
    }

    #endregion MUTATION
}
