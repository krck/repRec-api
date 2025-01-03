using Microsoft.AspNetCore.Mvc;
using RepRecApi.Common.Enums;
using RepRecApi.Database;
using RepRecApi.Models;
using Newtonsoft.Json;

#pragma warning disable CS8618 // Non-nullable field 
public class FreeExerciseDbImport
{
    public string Name { get; set; }
    public string Force { get; set; }
    public string Level { get; set; }
    public string Mechanic { get; set; }
    public string Equipment { get; set; }
    public List<string> PrimaryMuscles { get; set; }
    public List<string> SecondaryMuscles { get; set; }
    public List<string> Instructions { get; set; }
    public string Category { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field


[ApiController]
[Route("api/import")]
public class ImportController : ControllerBase
{
    private readonly RepRecDbContext _context;

    public ImportController(RepRecDbContext context)
    {
        _context = context;
    }

    public string? FirstCharToUpper(string? input) =>
    input switch
    {
        null => input,
        "" => input,
        _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
    };


    [HttpGet("freeExerciseDb")]
    public async Task<IActionResult> GetFreeExerciseDb()
    {
        // https://github.com/yuhonas/free-exercise-db/
        string _jsonDirectory = "/home/vboxuser/Downloads/free-exercise-db-main/exercises";
        try
        {
            // Ensure the directory exists
            if (!Directory.Exists(_jsonDirectory))
                return NotFound("Directory not found.");

            // Parse JSON files into Exercise objects
            var exercises = new List<OptExercise>();

            // Get all .json files in the directory
            var jsonFiles = Directory.GetFiles(_jsonDirectory, "*.json");
            foreach (var file in jsonFiles)
            {
                var content = System.IO.File.ReadAllText(file);
                var exercise = JsonConvert.DeserializeObject<FreeExerciseDbImport>(content);
                if (exercise != null)
                {
                    exercises.Add(new OptExercise
                    {
                        Id = 0,
                        Name = exercise.Name,
                        OptExerciseCategoryId = exercise.Category switch
                        {
                            "plyometrics" => (int)EnumExerciseCategories.Plyometrics,
                            "strongman" => (int)EnumExerciseCategories.Strongman,
                            "cardio" => (int)EnumExerciseCategories.Endurance_Training,
                            "stretching" => (int)EnumExerciseCategories.Stretching,
                            "olympic weightlifting" => (int)EnumExerciseCategories.Olympic_Lifting,
                            "powerlifting" => (int)EnumExerciseCategories.Weightlifting,
                            "strength" => (int)EnumExerciseCategories.Weightlifting,
                            _ => (int)EnumExerciseCategories.Other_Activities
                        },
                        Level = FirstCharToUpper(exercise.Level) ?? "Intermediate",
                        PrimaryMuscles = FirstCharToUpper(string.Join(",", exercise.PrimaryMuscles)) ?? "Unknown",
                        SecondaryMuscles = FirstCharToUpper(exercise.SecondaryMuscles.Count > 0 ? string.Join(",", exercise.SecondaryMuscles) : null),
                        Mechanic = FirstCharToUpper(exercise.Mechanic ?? "Unknown"),
                        Force = FirstCharToUpper(exercise.Force),
                        Equipment = FirstCharToUpper(exercise.Equipment),
                        Instructions = (exercise.Instructions.Count > 0 ? string.Join(".", exercise.Instructions).Replace("..", ". ") : null),
                    });
                }
            }

            // Save to database
            _context.OptExercises.AddRange(exercises);
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}
