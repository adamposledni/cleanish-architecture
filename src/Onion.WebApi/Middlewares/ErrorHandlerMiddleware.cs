using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Onion.Application.DataAccess.Localization;
using Onion.Core.Exceptions;
using Onion.Core.Helpers;
using Onion.WebApi.Models;

namespace Onion.WebApi.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IStringLocalizer<Strings> localizer, ILogger<ErrorHandlerMiddleware> logger)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            var error = HandleException(ex, localizer);
            if (error.StatusCode == 500) logger.LogError("{ex}", ex);
            httpContext.Response.StatusCode = error.StatusCode;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(error);
        }
    }

    public ErrorRes HandleException(Exception ex, IStringLocalizer<Strings> localizer)
    {
        Guard.NotNull(ex, nameof(ex));
        Guard.NotNull(localizer, nameof(localizer));

        return ex switch
        {
            NotFoundException notFoundException => new ErrorRes(404, localizer[notFoundException.MessageKey]),
            BadRequestException badRequestException => new ErrorRes(400, localizer[badRequestException.MessageKey], badRequestException.Details),
            UnauthorizedException unauthorizedException => new ErrorRes(401, localizer[unauthorizedException.MessageKey]),
            ForbiddenException forbiddenException => new ErrorRes(403, localizer[forbiddenException.MessageKey]),
            _ => new ErrorRes(500, localizer["ServerErrorMessage"]),
        };
    }
}
