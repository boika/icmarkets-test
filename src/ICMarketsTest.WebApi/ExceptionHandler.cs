using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Polly.RateLimiting;

namespace ICMarketsTest.WebApi;

/// <summary>
/// Global exception handler for catching and logging unhandled exceptions
/// and returning unified error response
/// </summary>
public sealed class ExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<ExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", httpContext.TraceIdentifier);

        var (statusCode, title) = MapException(exception);
        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails
            {
                Title = title,
                Status = statusCode,
                Extensions = { ["traceId"] = httpContext.TraceIdentifier }
            }
        };

        httpContext.Response.StatusCode = statusCode;
        return await _problemDetailsService.TryWriteAsync(problemDetailsContext);
    }

    private static (int StatusCode, string Title) MapException(Exception exception) => exception switch
    {
        RateLimiterRejectedException => (StatusCodes.Status429TooManyRequests, "Too many requests"),
        _ => (StatusCodes.Status500InternalServerError, "Internal server error")
    };
}