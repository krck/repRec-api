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
    public required DbSet<User> Users { get; set; }
    public required DbSet<Role> Roles { get; set; }
    public required DbSet<UserRole> UserRoles { get; set; }

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

        // Seed all initial/hardcoded values
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = (int)EnumRoles.Admin, Name = EnumRoles.Admin.ToString() },
            new Role { Id = (int)EnumRoles.Planner, Name = EnumRoles.Planner.ToString() },
            new Role { Id = (int)EnumRoles.User, Name = EnumRoles.User.ToString() }
        );
    }

}
