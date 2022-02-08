using Microsoft.AspNetCore.Builder;
using Onion.WebApi.Middlewares;

namespace Onion.WebApi.Extensions;

public static class ApplicationBuilderExtensions
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
