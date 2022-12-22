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
    string method = httpContext.Request.Method;
    string path = httpContext.Request.Path.Value;
    int statusCode = httpContext.Response.StatusCode;
    string message = $"HTTP {method} {path} responded with {statusCode}";
    _logger.LogInformation("{message}", message);
}
}
