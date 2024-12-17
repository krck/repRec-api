using Microsoft.EntityFrameworkCore;
using RepRecApi.Common.Enums;
using RepRecApi.Models;

namespace RepRecApi.Database;

//
// Entity Framework Core DbContext for the RepRec database
//
public class RepRecDbContext(DbContextOptions<RepRecDbContext> options) : DbContext(options)
// Primary constructor in declaration
{
#pragma warning disable CS8618 // Non-nullable field 

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RepRecApi.Models.Log> Logs { get; set; }
    public DbSet<RepRecApi.Models.LogLevel> LogLevels { get; set; }


#pragma warning restore CS8618 // Non-nullable field

    // Configure relationships in OnModelCreating
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });  // Composite key for UserRole

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<RepRecApi.Models.Log>()
            .HasOne(l => l.LogLevel)
            .WithMany(ll => ll.Logs)
            .HasForeignKey(l => l.LogLevelId);

        // Seed all initial/hardcoded values
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = (int)EnumRoles.Admin, Name = EnumRoles.Admin.ToString() },
            new Role { Id = (int)EnumRoles.Planner, Name = EnumRoles.Planner.ToString() },
            new Role { Id = (int)EnumRoles.User, Name = EnumRoles.User.ToString() }
        );
        modelBuilder.Entity<RepRecApi.Models.LogLevel>().HasData(
            new RepRecApi.Models.LogLevel { Id = (int)EnumLogLevels.Debug, Name = EnumLogLevels.Debug.ToString() },
            new RepRecApi.Models.LogLevel { Id = (int)EnumLogLevels.Info, Name = EnumLogLevels.Info.ToString() },
            new RepRecApi.Models.LogLevel { Id = (int)EnumLogLevels.Warning, Name = EnumLogLevels.Warning.ToString() },
            new RepRecApi.Models.LogLevel { Id = (int)EnumLogLevels.Error, Name = EnumLogLevels.Error.ToString() },
            new RepRecApi.Models.LogLevel { Id = (int)EnumLogLevels.Critical, Name = EnumLogLevels.Critical.ToString() }
        );
    }

}
