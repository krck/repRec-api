
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using RepRecApi.Database;
using RepRecApi.Common.Enums;

namespace RepRecApi.Common.Services;

/// <summary>
/// User Service Interface
/// (Used for DI - Singleton)
/// </summary>
public interface IUserService
{
    Task<List<EnumRoles>> GetUserRolesAsync(string userId);
}

/// <summary>
/// UserService
/// Load and cache User Role data
/// </summary>
public class UserService : IUserService
{
    private readonly IMemoryCache _cache;
    private readonly IDbService _dbService;

    public UserService(IMemoryCache cache, IDbService dbService)
    {
        _cache = cache;
        _dbService = dbService;
    }

    public async Task<List<EnumRoles>> GetUserRolesAsync(string userId)
    {
        // Try to get roles from cache
        if (_cache.TryGetValue(userId, out List<EnumRoles>? roles) && roles != null)
            return roles;

        // Fetch roles from the database
        var dbUserRoles = await _dbService.GetDbContext().UserRoles.Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).ToListAsync();
        roles = dbUserRoles.Select(r => (EnumRoles)r).ToList();

        // Cache the roles for 15 minutes absolute, 5 minutes sliding
        _cache.Set(userId, roles, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
            SlidingExpiration = TimeSpan.FromMinutes(5)
        });

        return roles;
    }
}
