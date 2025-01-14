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

    // CORE System Tables
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RepRecApi.Models.Log> Logs { get; set; }
    public DbSet<RepRecApi.Models.LogLevel> LogLevels { get; set; }

    // PLANNING Tables
    public DbSet<PlanWorkout> PlanWorkouts { get; set; }
    public DbSet<PlanWorkoutExercise> PlanWorkoutExercises { get; set; }

    // OPTIONS / Dropdown Tables
    public DbSet<OptExerciseCategory> OptExerciseCategories { get; set; }
    public DbSet<OptExercise> OptExercises { get; set; }


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

        modelBuilder.Entity<OptExercise>()
            .HasOne(e => e.OptExerciseCategory)
            .WithMany(c => c.OptExercises)
            .HasForeignKey(e => e.OptExerciseCategoryId);

        modelBuilder.Entity<PlanWorkout>()
            .HasOne(pw => pw.User)
            .WithMany(u => u.PlanWorkouts)
            .HasForeignKey(pw => pw.UserId);

        modelBuilder.Entity<PlanWorkoutExercise>()
            .HasOne(pwe => pwe.PlanWorkout)
            .WithMany(pw => pw.PlanWorkoutExercises)
            .HasForeignKey(pwe => pwe.PlanWorkoutId);

        modelBuilder.Entity<PlanWorkoutExercise>()
            .HasOne(pwe => pwe.OptExerciseCategory)
            .WithMany(c => c.PlanWorkoutExercises)
            .HasForeignKey(pwe => pwe.OptExerciseCategoryId);

        modelBuilder.Entity<PlanWorkoutExercise>()
            .HasOne(pwe => pwe.OptExercise)
            .WithMany(e => e.PlanWorkoutExercises)
            .HasForeignKey(pwe => pwe.OptExerciseId);

        modelBuilder.Entity<PlanWorkoutExercise>()
            .HasOne(pwe => pwe.User)
            .WithMany(u => u.PlanWorkoutExercises)
            .HasForeignKey(pwe => pwe.UserId);


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
        modelBuilder.Entity<OptExerciseCategory>().HasData(
            new OptExerciseCategory
            {
                Id = (int)EnumExerciseCategories.Weightlifting,
                Name = EnumExerciseCategories.Weightlifting.ToString().Replace("_", " "),
                Description = "Bodybuilding and Power-Lifting exercises",
                JsonTemplate = "[]"
            }
        );
        modelBuilder.Entity<OptExerciseCategory>().HasData(
            new OptExerciseCategory
            {
                Id = (int)EnumExerciseCategories.Olympic_Lifting,
                Name = EnumExerciseCategories.Olympic_Lifting.ToString().Replace("_", " "),
                Description = "Snatch, Clean & Jerk, etc.",
                JsonTemplate = "[]"
            }
        );
        modelBuilder.Entity<OptExerciseCategory>().HasData(
            new OptExerciseCategory
            {
                Id = (int)EnumExerciseCategories.Strongman,
                Name = EnumExerciseCategories.Strongman.ToString().Replace("_", " "),
                Description = "Atlas Stones, Tire Flips, etc.",
                JsonTemplate = "[]"
            }
        );
        modelBuilder.Entity<OptExerciseCategory>().HasData(
            new OptExerciseCategory
            {
                Id = (int)EnumExerciseCategories.Plyometrics,
                Name = EnumExerciseCategories.Plyometrics.ToString().Replace("_", " "),
                Description = "Box Jumps, Jump Squats, etc.",
                JsonTemplate = "[]"
            }
        );
        modelBuilder.Entity<OptExerciseCategory>().HasData(
            new OptExerciseCategory
            {
                Id = (int)EnumExerciseCategories.Stretching,
                Name = EnumExerciseCategories.Stretching.ToString().Replace("_", " "),
                Description = "Static, Dynamic, PNF, etc.",
                JsonTemplate = "[]"
            }
        );
        modelBuilder.Entity<OptExerciseCategory>().HasData(
            new OptExerciseCategory
            {
                Id = (int)EnumExerciseCategories.Endurance_Training,
                Name = EnumExerciseCategories.Endurance_Training.ToString().Replace("_", " "),
                Description = "All Forms of Cardio",
                JsonTemplate = "[]"
            }
        );
        modelBuilder.Entity<OptExerciseCategory>().HasData(
            new OptExerciseCategory
            {
                Id = (int)EnumExerciseCategories.Physical_Exercises,
                Name = EnumExerciseCategories.Physical_Exercises.ToString().Replace("_", " "),
                Description = "Yoga, Pilates, Calisthenics, Courses, etc.",
                JsonTemplate = "[]"
            }
        );
        modelBuilder.Entity<OptExerciseCategory>().HasData(
            new OptExerciseCategory
            {
                Id = (int)EnumExerciseCategories.Other_Activities,
                Name = EnumExerciseCategories.Other_Activities.ToString().Replace("_", " "),
                Description = "Hiking, Swimming, Bouldering, Outdoor, etc.",
                JsonTemplate = "[]"
            }
        );
    }

}
