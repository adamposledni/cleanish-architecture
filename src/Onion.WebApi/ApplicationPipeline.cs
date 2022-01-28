using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Onion.WebApi.Middlewares;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace Onion.WebApi;

public static class ApplicationPipeline
{
    public static void Setup(WebApplication app)
    {
        // clear default claim mappings
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        app.UseMiddleware<HttpLoggingMiddleware>();

        var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(localizationOptions.Value);

        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseHsts();

        app.UseHttpsRedirection();

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint(
            "/swagger/v1/swagger.json", 
            $"{Assembly.GetAssembly(typeof(ApplicationPipeline)).FullName} v1"));

        app.UseStaticFiles();
        app.UseSpaStaticFiles();


        app.UseRouting();
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health");
            endpoints.MapControllers();
        });

        app.UseSpa(c =>
        {
            c.Options.SourcePath = "wwwroot";
        });

        app.Run();
    }
}
