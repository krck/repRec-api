using Microsoft.AspNetCore.Mvc;
using System.Reflection;

[ApiController]
[Route("api/version")]
public class VersionController : ControllerBase
{
    [HttpGet]
    // Not Authenticated / not Authorized
    // Can be used as a simple test route
    public IActionResult Get()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown version";
        return Ok(new { version });
    }
}
