
using Microsoft.EntityFrameworkCore;
using RepRecApi.Database;

namespace RepRecApi.Common.Services;

/// <summary>
/// Db Service Interface
/// (Used for DI - Singleton)
/// </summary>
public interface IDbService
{
    RepRecDbContext GetDbContext();
}

/// <summary>
/// UserService
/// Load and cache User Role data
/// </summary>
public class DbService : IDbService
{
    private readonly string _connectionString;

    public DbService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public RepRecDbContext GetDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<RepRecDbContext>();
        var options = optionsBuilder.UseNpgsql(_connectionString).Options;

        return new RepRecDbContext(options);
    }

}
