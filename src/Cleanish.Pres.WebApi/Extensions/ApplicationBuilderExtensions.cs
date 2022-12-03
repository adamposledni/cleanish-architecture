using Microsoft.AspNetCore.Builder;
using Cleanish.Pres.WebApi.Middlewares;

namespace Cleanish.Pres.WebApi.Extensions;

internal static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseHttpRequesLogging(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<HttpLoggingMiddleware>();
    }

    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<ErrorHandlerMiddleware>();
    }
}