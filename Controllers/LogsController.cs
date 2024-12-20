using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RepRecApi.Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using RepRecApi.Common.Enums;
using RepRecApi.Models.DTOs;
using RepRecApi.Database;

namespace RepRecApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly RepRecDbContext _context;

    public LogsController(RepRecDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GET: api/logs
    /// 
    /// Get all (Error)Logs with their assigned users/logLevels
    /// (This is only available to Admins)
    /// </summary>
    [HttpGet]
    [Authorize]
    [RoleAccess(EnumRoles.Admin)]
    public async Task<ActionResult<IEnumerable<LogDto>>> GetLogs([FromQuery] int filterType)
    {
        // validate filterType (1=Last Hour, 2=Last Day, 3=Last Week)
        if (filterType < 1 || filterType > 3)
            throw new ValidationException("Invalid filterType");

        // Get all Log data based on the filterValue and left join User name on the Log.userId
        var logQuery = (from log in _context.Logs
                        from user in _context.Users.Where(u => u.Id == log.userId).DefaultIfEmpty() // LEFT JOIN
                        join level in _context.LogLevels on log.LogLevelId equals level.Id          // INNER JOIN
                        where (
                            (filterType == 1 && log.Timestamp >= DateTime.UtcNow.AddHours(-1))     // Last Hour
                            || (filterType == 2 && log.Timestamp >= DateTime.UtcNow.Date)          // Last Day
                            || (filterType == 3 && log.Timestamp >= DateTime.UtcNow.AddDays(-7))   // Last Week
                        )
                        select new LogDto
                        {
                            // log.Id,
                            Timestamp = log.Timestamp,
                            Level = level.Name,
                            UserName = user.Nickname,
                            ExceptionType = log.ExceptionType,
                            Message = log.Message,
                            Source = log.Source,
                            // log.StackTrace,
                        })
                        .OrderByDescending(l => l.Timestamp);

        var result = await logQuery.ToListAsync();
        return Ok(result);
    }

}
