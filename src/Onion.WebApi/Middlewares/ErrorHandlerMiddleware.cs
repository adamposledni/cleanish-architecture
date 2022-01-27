using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Onion.Application.DataAccess.Exceptions.Auth;
using Onion.Application.DataAccess.Exceptions.Common;
using Onion.WebApi.Models;
using Onion.WebApi.Resources;

namespace Onion.WebApi.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IStringLocalizer<Resource> localizer, ILogger<ErrorHandlerMiddleware> logger)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            var error = HandleException(ex, localizer, logger);
            httpContext.Response.StatusCode = error.StatusCode;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(error);
        }
    }

    public ErrorRes HandleException(Exception ex, IStringLocalizer<Resource> localizer, ILogger<ErrorHandlerMiddleware> logger)
    {
        switch (ex)
        {
            case NotFoundException notFoundException:
                return new ErrorRes(404, localizer[notFoundException.MessageKey, notFoundException]);

            case BadRequestException badRequestException:
                return new ErrorRes(400, localizer[badRequestException.MessageKey], badRequestException.Details);

            case UnauthorizedException unauthorizedException:
                return new ErrorRes(401, localizer[unauthorizedException.MessageKey]);

            case ForbiddenException forbiddenException:
                return new ErrorRes(403, localizer[forbiddenException.MessageKey]);

            default:
                logger.LogError("{ex}", ex);
                return new ErrorRes(500, localizer["ServerErrorMessage"]);
        }
    }
}
