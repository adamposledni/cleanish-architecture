using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Onion.Application.Services.Exceptions;
using Onion.WebApi.Models;
using Onion.WebApi.Resources;
using System;
using System.Threading.Tasks;

namespace Onion.WebApi.Middlewares
{
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
                int statusCode;
                string message;

                switch (ex)
                {
                    case NotFoundException notFoundException:
                        statusCode = 404;
                        message = localizer[notFoundException.MessageKey, notFoundException.Id];
                        break;

                    case BadRequestException badRequestException:
                        statusCode = 400;
                        message = badRequestException.Message;
                        break;

                    default:
                        statusCode = 500;
                        message = localizer["ServerErrorMessage"];
                        logger.LogError(ex.ToString());
                        break;
                }

                httpContext.Response.StatusCode = statusCode;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsJsonAsync(new ErrorRes(statusCode, message));
            }
        }
    }
}
