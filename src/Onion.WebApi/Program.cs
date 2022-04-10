using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Onion.Application.Services;
using Onion.Infrastructure.Core;
using Onion.Infrastructure.DataAccess;
using Onion.WebApi;
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
configuration.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
configuration.AddEnvironmentVariables();

// logging setup
logging.ClearProviders();
logging.AddConsole();

// services registration
CoreLayer.Compose(services, configuration);
DataAccessLayer.Compose(services, configuration);
ApplicationLayer.Compose(services, configuration);
WebApiLayer.Compose(services, configuration);

// application pipeline
var app = builder.Build();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

app.UseHttpRequesLogging();

var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);

app.UseErrorHandler();

if (!env.IsDevelopment()) app.UseHsts();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint(
    "/swagger/v1/swagger.json",
    $"{Assembly.GetAssembly(typeof(Program)).FullName} v1"));

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

app.UseSpa(c => c.Options.SourcePath = "wwwroot");

var logger = app.Services.GetService<ILogger<Program>>();
logger.LogInformation("Web application was successfully created");

app.Run();