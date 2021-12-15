using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Onion.Application.DataAccess.Exceptions;
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
                ErrorRes error = new();

                switch (ex)
                {
                    case NotFoundException notFoundException:
                        error.StatusCode = 404;
                        error.Message = localizer[notFoundException.MessageKey, notFoundException.Id];
                        break;

                    case BadRequestException badRequestException:
                        error.StatusCode = 400;
                        error.Message = localizer[badRequestException.MessageKey];
                        error.ServerDetails = badRequestException.Details;
                        break;

                    default:
                        error.StatusCode = 500;
                        error.Message = localizer["ServerErrorMessage"];
                        logger.LogError(ex.ToString());
                        break;
                }

                httpContext.Response.StatusCode = error.StatusCode;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
