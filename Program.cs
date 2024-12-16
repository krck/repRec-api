using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using RepRecApi.Common.Services;
using RepRecApi.Database;


// --------------------------------------------------------------------------
// --------------------- Add SERVICES to the container ----------------------
// --------------------------------------------------------------------------

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowRepRecOrigins", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",            // Development Frontend
            "https://www.reprec.de"             // Production Frontend (Domain)
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials(); // Allow cookies/auth headers if needed
    });
});

string? apiAuthority = builder.Configuration.GetValue<string>("API_AUTHORITY");
string? apiAudience = builder.Configuration.GetValue<string>("API_AUDIENCE");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = apiAuthority;
        options.Audience = apiAudience;
        options.RequireHttpsMetadata = false; // Set to true in production
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                // For debugging:
                // Console.WriteLine("Authentication failed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
        };
    });
builder.Services.AddAuthorization();

// setup the EF Postgres database connection
string connectionString = builder.Configuration.GetValue<string>("DB_CONNECTION_DEV") ?? "";
builder.Services.AddDbContext<RepRecDbContext>(options => options.UseNpgsql(connectionString));

// Add all DI Services
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IDbService>(new DbService(connectionString));
builder.Services.AddSingleton<IUserService, UserService>();
//builder.Services.AddScoped();
//builder.Services.AddTransient();

// Add the REST API controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
;

// --------------------------------------------------------------------------
// ----------------------- Create the WebApplication ------------------------
// --------------------------------------------------------------------------
var app = builder.Build();


// --------------------------------------------------------------------------
// --------------------- Add MIDDLEWARE to the pipeline ---------------------
// --------------------------------------------------------------------------
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}
app.UseRouting();
app.UseCors("AllowRepRecOrigins"); // CORS: Must be placed after UseRouting and before UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.Run();
