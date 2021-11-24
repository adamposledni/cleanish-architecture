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
using Onion.Core.Mapper;
using Onion.Infrastructure.Mapper;
using Onion.Infrastucture.DataAccess;
using Onion.Infrastucture.DataAccess.MongoDb;
using Onion.Infrastucture.DataAccess.MongoDb.Configuration;
using Onion.Infrastucture.DataAccess.Sql;
using Onion.Infrastucture.DataAccess.Sql.Repositories;
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
        private ApplicationSettings _applicationSettings;
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
                .ConfigureApiBehaviorOptions(opt =>
                {
                    //opt.SuppressMapClientErrors = true;
                });

            ConfigureApplicationSettings(services);
            ConfigureLocalization(services);
            ConfigureAuthentication(services);
            ConfigureHealthChecks(services);
            ConfigureSwagger(services);
            ConfigureCors(services);
            ConfigureDataAccess(services);
            ConfigureLogic(services);
            ConfigureInfrastructure(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
            app.UseHttpLogging();
            app.UseExceptionHandler(
                new ExceptionHandlerOptions()
                {
                    AllowStatusCode404Response = true,
                    ExceptionHandlingPath = "/error"
                }
            );

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

        private void ConfigureLocalization(IServiceCollection services)
        {
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("cs-CZ")
            };

            services.Configure<RequestLocalizationOptions>(opt =>
            {
                opt.DefaultRequestCulture = new RequestCulture(supportedCultures[1]);
                opt.SupportedCultures = supportedCultures;
                opt.SupportedUICultures = supportedCultures;
                opt.RequestCultureProviders = new[] { new AcceptLanguageHeaderRequestCultureProvider() };
            });

            services.AddLocalization();
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
            var key = Encoding.ASCII.GetBytes(_applicationSettings.JwtSigningKey);

            services
                .AddAuthentication(x =>
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
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

        private void ConfigureApplicationSettings(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("ApplicationSettings");
            services.Configure<ApplicationSettings>(appSettingsSection);
            _applicationSettings = appSettingsSection.Get<ApplicationSettings>();
        }

        private void ConfigureHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks();
        }

        private void ConfigureDataAccess(IServiceCollection services)
        {
            services.AddDbContext<SqlDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("Default"));
            });


            services.AddScoped<IMongoDbContext, MongoDbContext>((services) =>
            {
                var mongoDbSettings = Configuration.GetSection("MongoDbSettings:Default").Get<MongoDbSettings>();
                return new MongoDbContext(mongoDbSettings);
            });

            services.AddScoped<IRepositoryManager, RepositoryManager>();
            //services.AddScoped<ITransactionalRepositoryManager, TransactionalRepositoryManager>();
        }

        private void ConfigureInfrastructure(IServiceCollection services)
        {
            services.AddScoped<IMapper, Mapper>();
        }

        private void ConfigureLogic(IServiceCollection services)
        {
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IUserService, UserService>();
        }


    }
}
