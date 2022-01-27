using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Onion.WebApi.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var logging = builder.Logging;
var services = builder.Services;
var env = builder.Environment;

// configuration providers

configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
configuration.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
configuration.AddEnvironmentVariables();

// logging setup

logging.ClearProviders();
logging.AddConsole();

// services registration

services.AddControllersSetup();
services.AddHttpContextAccessor();
services.AddCorsSetup();
services.AddHealthChecksSetup();
services.AddSwaggerSetup();
services.AddLocalizationSetup();
services.AddJwtAuthentication(configuration);
services.AddDataAccess(configuration);
services.AddApplicationServices();
services.AddCoreServices(configuration);
services.AddSpa();

// application pipeline

var app = builder.Build();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

app.UseHttpRequestLogging();

var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);

app.UseErrorHandler();
app.UseHsts();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Assembly.GetExecutingAssembly().GetName().Name} v1"));

app.UseStaticFiles();
app.UseSpaStaticFiles();


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

app.Run();