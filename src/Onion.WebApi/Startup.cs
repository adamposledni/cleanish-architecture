using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Onion.WebApi.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace Onion.WebApi;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersSetup();
        services.AddHttpContextAccessor();
        services.AddCorsSetup();
        services.AddHealthChecksSetup();
        services.AddSwaggerSetup();
        services.AddLocalizationSetup();
        services.AddJwtAuthentication(Configuration);
        services.AddDataAccess(Configuration);
        services.AddApplicationServices();
        services.AddCoreServices(Configuration);
        services.AddSpa();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        app.UseHttpRequestLogging();
        app.UseErrorHandler();

        app.UseHttpsRedirection();

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Assembly.GetExecutingAssembly().GetName().Name} v1"));

        app.UseStaticFiles();
        app.UseSpaStaticFiles();

        var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(localizationOptions.Value);

        app.UseRouting();
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });

        app.UseSpa(c =>
        {
            c.Options.SourcePath = "wwwroot";
        });
    }
}
