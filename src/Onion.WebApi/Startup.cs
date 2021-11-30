using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Onion.Application.DataAccess.Configuration;
using Onion.Application.DataAccess.Repositories;
using Onion.Application.Services.Abstractions;
using Onion.Application.Services.Implementations;
using Onion.Core.Clock;
using Onion.Core.Mapper;
using Onion.Core.Security;
using Onion.Infrastructure.Clock;
using Onion.Infrastructure.Mapper;
using Onion.Infrastructure.Security;
using Onion.Infrastucture.DataAccess;
using Onion.Infrastucture.DataAccess.MongoDb;
using Onion.Infrastucture.DataAccess.MongoDb.Configuration;
using Onion.Infrastucture.DataAccess.Sql;
using Onion.Infrastucture.DataAccess.Sql.Repositories;
using Onion.WebApi.Extensions;
using Onion.WebApi.Middlewares;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Onion.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersSetup();
            services.AddApplicationSettings(Configuration);
            services.AddLocalizationSetup();
            services.AddJwtAuthentication(Configuration);
            services.AddHealthChecksSetup();
            services.AddSwaggerSetup();
            services.AddCorsSetup();
            services.AddDataAccess(Configuration);
            services.AddApplicationServices();
            services.AddCoreServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
            app.UseHttpLogging();
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Onion.WebApi v1"));

            // app.UseStaticFiles();
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

            logger.LogInformation("HTTP pipeline configured");
        }
    }
}
