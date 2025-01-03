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
public class OptExerciseCategoryController : ControllerBase
{
    private readonly RepRecDbContext _context;

    public OptExerciseCategoryController(RepRecDbContext context)
    {
        _context = context;
    }

    #region QUERY

    /// <summary>
    /// GET: api/OptExerciseCategory
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<OptExerciseCategory>>> GetOptExerciseCategories()
    {
        var allOptExerciseCategories = await _context.OptExerciseCategories.ToListAsync();
        return Ok(allOptExerciseCategories);
    }

    /// <summary>
    /// GET: api/OptExerciseCategory/5
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<OptExerciseCategory>> GetOptExerciseCategory(int id)
    {
        var optExerciseCategory = await _context.OptExerciseCategories.FindAsync(id);
        if (optExerciseCategory == null)
            throw new ValidationException("Unknown OptExerciseCategory Id");

        return Ok(optExerciseCategory);
    }

    #endregion QUERY

    #region MUTATION

    /// <summary>
    /// PUT: api/OptExerciseCategory/5
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    [RoleAccess(EnumRoles.Admin)]
    public async Task<ActionResult<OptExerciseCategory>> PutOptExerciseCategory(int id, OptExerciseCategory optExerciseCategory)
    {
        if (id != optExerciseCategory.Id)
            throw new ValidationException("Invalid OptExerciseCategory Id");

        // If its a valid Entity, let EF handle the update automatically
        _context.Entry(optExerciseCategory).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(optExerciseCategory);
    }

    /// <summary>
    /// POST: api/OptExerciseCategory
    /// </summary>
    [HttpPost]
    [Authorize]
    [RoleAccess(EnumRoles.Admin)]
    public async Task<ActionResult<OptExerciseCategory>> PostOptExerciseCategory(OptExerciseCategory optExerciseCategory)
    {
        var dbItem = _context.OptExerciseCategories.Add(optExerciseCategory);
        await _context.SaveChangesAsync();

        return Ok(dbItem.Entity);
    }

    /// <summary>
    /// DELETE: api/OptExerciseCategory/5
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    [RoleAccess(EnumRoles.Admin)]
    public async Task<ActionResult<OptExerciseCategory>> DeleteOptExerciseCategory(int id)
    {
        var optExerciseCategory = await _context.OptExerciseCategories.FindAsync(id);
        if (optExerciseCategory == null)
            throw new ValidationException("Unknown OptExerciseCategory Id");

        _context.OptExerciseCategories.Remove(optExerciseCategory);
        await _context.SaveChangesAsync();

        return Ok(optExerciseCategory);
    }

    #endregion MUTATION
}
