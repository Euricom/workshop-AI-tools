using System.Net;
using System.Text.Json;
using MyApp.Application.Exceptions;

namespace MyApp.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            TraceId = context.TraceIdentifier
        };

        switch (exception)
        {
            case ValidationException validationEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = "Validation failed";
                errorResponse.Details = validationEx.Errors;
                break;

            case NotFoundException notFoundEx:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Message = notFoundEx.Message;
                break;

            case AuthenticationException authEx:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.Message = authEx.Message;
                break;

            case AuthorizationException forbiddenEx:
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                errorResponse.Message = forbiddenEx.Message;
                break;

            case BusinessRuleException businessEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = businessEx.Message;
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "An unexpected error occurred";
                if (_environment.IsDevelopment())
                {
                    errorResponse.Details = new Dictionary<string, string[]>
                    {
                        { "ExceptionMessage", new[] { exception.Message } },
                        { "StackTrace", new[] { exception.StackTrace ?? "No stack trace available" } }
                    };
                }
                break;
        }

        var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(result);
    }

    private class ErrorResponse
    {
        public string TraceId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public IDictionary<string, string[]>? Details { get; set; }
    }
}

// Extension method for easy middleware registration
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}