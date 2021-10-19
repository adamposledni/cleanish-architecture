using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Onion.Application.Domain;
using Onion.Application.Domain.Configuration;
using Onion.Application.Domain.Repositories;
using Onion.Application.Services.Abstractions;
using Onion.Application.Services.Implementations;
using Onion.Core.Mapper;
using Onion.Infrastructure.Mapper;
using Onion.Infrastucture.Persistence;
using Onion.Infrastucture.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            services.AddControllers()
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    opt.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .ConfigureApiBehaviorOptions(o =>
                {
                    //o.SuppressMapClientErrors = true;
                });

            ConfigureApplicationSettings(services);
            ConfigureAuthentication(services);
            ConfigureSwagger(services);
            ConfigureCors(services);
            ConfigurePersistance(services);
            ConfigureLogic(services);
            ConfigureInfrastructure(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
            logger.LogInformation("Server is running");
            if (_env.IsDevelopment()) logger.LogInformation("Running in development");

            app.UseExceptionHandler("/error");

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Onion.WebApi v1"));

            // app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            // app.UseCustomMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            logger.LogInformation("HTTP pipeline configured");
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Onion.WebApi", 
                    Version = "v1" 
                });
            });
        }

        private void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(conf =>
            {
                conf.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
            });
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            string key = Configuration.GetValue<string>($"{Constants.APPLICATION_SETTINGS_SECTION}:JwtSigningKey");

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

        private void ConfigureApplicationSettings(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection(Constants.APPLICATION_SETTINGS_SECTION);
            services.Configure<ApplicationSettings>(appSettingsSection);
        }

        private void ConfigurePersistance(IServiceCollection services)
        {
            services.AddDbContext<OnionDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        private void ConfigureInfrastructure(IServiceCollection services)
        {
            services.AddScoped<IMapper, Mapper>();
        }

        private void ConfigureLogic(IServiceCollection services)
        {
            services.AddScoped<IItemService, ItemService>();
        }
    }
}
