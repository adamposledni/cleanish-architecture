using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cleanish.Pres.WebApi.Middlewares;

internal class HttpLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HttpLoggingMiddleware> _logger;

    public HttpLoggingMiddleware(RequestDelegate next, ILogger<HttpLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        await _next(httpContext);
        var request = httpContext.Request;
        var response = httpContext.Response;
        string message = $"HTTP {request.Method} {request.Path.Value} responded with {response.StatusCode}";
        _logger.LogInformation("{message}", message);
    }
}
