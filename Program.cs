var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------------------------------
// --------------------- Add SERVICES to the container ----------------------
// --------------------------------------------------------------------------
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()  // Allows any origin (you can restrict this to specific origins)
              .AllowAnyMethod()  // Allows any HTTP method (GET, POST, PUT, DELETE, etc.)
              .AllowAnyHeader(); // Allows any HTTP header
    });
});

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
app.UseCors(); // CORS: Must be placed after UseRouting and before UseAuthorization
app.UseAuthorization();
app.MapControllers();

app.Run();
