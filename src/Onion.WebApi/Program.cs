using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Onion.Application.Services;
using Onion.Infrastructure.Core;
using Onion.Infrastructure.DataAccess;
using Onion.WebApi;

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
WebApiLayer.Compose(services, configuration);
ApplicationLayer.Compose(services, configuration);
DataAccessLayer.Compose(services, configuration);
CoreLayer.Compose(services, configuration);

// application pipeline
ApplicationPipeline.Setup(builder.Build());