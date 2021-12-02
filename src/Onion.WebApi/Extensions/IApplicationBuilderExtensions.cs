using Microsoft.AspNetCore.Builder;
using Onion.WebApi.Middlewares;

namespace Onion.WebApi.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
