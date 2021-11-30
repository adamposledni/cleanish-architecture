using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
using Onion.Infrastucture.DataAccess.Sql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Onion.WebApi.Extensions
{
    public static class IServiceCollectionExtensions
    {

        public static void AddControllersSetup(this IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(opt =>
                {
                    //opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    opt.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .ConfigureApiBehaviorOptions(opt =>
                {
                    //opt.SuppressMapClientErrors = true;
                });
        }

        public static void AddApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetApplicationSettingsSection();
            services.Configure<ApplicationSettings>(appSettingsSection);
        }

        public static void AddLocalizationSetup(this IServiceCollection services)
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

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.ASCII.GetBytes(configuration.GetJwtSigningKey());

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

        public static void AddSwaggerSetup(this IServiceCollection services)
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

        public static void AddCorsSetup(this IServiceCollection services)
        {
            services.AddCors(conf =>
            {
                conf.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
            });
        }

        public static void AddHealthChecksSetup(this IServiceCollection services)
        {
            services.AddHealthChecks();
        }

        public static void AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            // SqlServer
            services.AddDbContext<SqlDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            // MongoDb
            MongoDbContext.Configure();
            services.AddScoped<IMongoDbContext, MongoDbContext>((services) =>
            {
                var mongoDbSettings = configuration.GetMongoDbSettings("Default");
                var clockProvider = services.GetRequiredService<IClockProvider>();
                return new MongoDbContext(mongoDbSettings, clockProvider);
            });

            // UoW
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            //services.AddScoped<ITransactionalRepositoryManager, TransactionalRepositoryManager>();
        }

        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IMapper, Mapper>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddTransient<IClockProvider, ClockProvider>();
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
