using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Net;
using RepRecApi.Common.Services;

namespace RepRecApi.Common.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogService _logService;
    private readonly string _auth0UserIdClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

    public GlobalExceptionMiddleware(RequestDelegate next, ILogService logService)
    {
        _next = next;
        _logService = logService;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Continue processing the request down the chain of middlewares
            // (within this global try/catch exception handler)
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the exception
            string? userId = context.User.Claims.FirstOrDefault(c => c.Type == _auth0UserIdClaim)?.Value;
            await _logService.LogExceptionAsync(ex, userId);

            // Create the response
            await CreateProblemResponse(context, ex);
        }
    }

    /// <summary>
    /// CreateProblemResponse
    /// 
    /// Create a outgoing response after an exception was caught
    /// (Using the RFC 7807 ProblemDetails format for standardizing error responses)
    /// </summary>
    private static Task CreateProblemResponse(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        // Response types - Only distinguish between:
        // - "400 BadRequest": In case of invalid inputs, validation errors
        // - "500 Internal Server Error": for all other unexpected, unhandled exceptions
        // (stuff like 401/403 for Auth, or 404 are handled by the framework already)
        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                title = "Bad Request",
                status = context.Response.StatusCode,
                detail = validationException.Message,
            }));
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                title = "Internal Server Error",
                status = context.Response.StatusCode,
                detail = "An unexpected error occurred.",
            }));
        }
    }

}
