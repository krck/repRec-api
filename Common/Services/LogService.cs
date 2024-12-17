using RepRecApi.Common.Enums;
using RepRecApi.Models;

namespace RepRecApi.Common.Services;

public interface ILogService
{
    Task LogExceptionAsync(Exception exception, string? userId = null);
    Task LogMessageAsync(EnumLogLevels logLevel, string message, string type, string? userId = null);
}

public class LogService : ILogService
{
    private readonly IDbService _dbService;

    public LogService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task LogExceptionAsync(Exception exception, string? userId = null)
    {
        try
        {
            var context = _dbService.GetDbContext();
            context.Logs.Add(new Log
            {
                Id = 0,
                LogLevelId = (int)EnumLogLevels.Error,
                Timestamp = DateTime.UtcNow,
                ExceptionType = exception.GetType().Name,
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Source = exception.Source,
                userId = userId
            });
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Handle logging failure (eg. fallback to file or console)
            Console.WriteLine($"Failed to log to DB: {ex.Message}");
        }
    }

    public async Task LogMessageAsync(EnumLogLevels logLevel, string message, string type, string? userId = null)
    {
        try
        {
            var context = _dbService.GetDbContext();
            context.Logs.Add(new Log
            {
                Id = 0,
                LogLevelId = (int)logLevel,
                Timestamp = DateTime.UtcNow,
                ExceptionType = type,
                Message = message,
                userId = userId
            });
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Handle logging failure (eg. fallback to file or console)
            Console.WriteLine($"Failed to log to DB: {ex.Message}");
        }
    }

}
