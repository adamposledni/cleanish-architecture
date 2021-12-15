using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Onion.Application.DataAccess.Repositories;
using Onion.Application.Services.Auth;
using Onion.Application.Services.Common;
using Onion.Application.Services.ItemManagement;
using Onion.Application.Services.UserManagement;
using Onion.Core.Clock;
using Onion.Core.Mapper;
using Onion.Core.Security;
using Onion.Infrastructure.Clock;
using Onion.Infrastructure.Mapper;
using Onion.Infrastructure.Security.Facebook;
using Onion.Infrastructure.Security.Google;
using Onion.Infrastructure.Security.Jwt;
using Onion.Infrastructure.Security.Password;
using Onion.Infrastucture.DataAccess;
using Onion.Infrastucture.DataAccess.MongoDb;
using Onion.Infrastucture.DataAccess.Sql;
using Onion.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Onion.WebApi.Extensions
{
    public static class IServiceCollectionExtensions
    {
        #region Controllers
        public static void AddControllersSetup(this IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .ConfigureApiBehaviorOptions(opt =>
                {
                    //opt.SuppressMapClientErrors = true;
                    opt.InvalidModelStateResponseFactory = ctx => ctx.HandleModelValidationErrors();
                });
        }
        #endregion

        #region App settings
        public static void AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            //var appSettingsSection = configuration.GetApplicationSettingsSection();
            //services.Configure<ApplicationSettings>(appSettingsSection);
        }
        #endregion

        #region Localization
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
        #endregion

        #region Authentication
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
        #endregion

        #region Swagger
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                //c.IncludeXmlComments(string.Format(@"{0}\Onion.WebApi.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Onion.WebApi",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });
        }
        #endregion

        #region CORS
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
        #endregion

        #region Health checks
        public static void AddHealthChecksSetup(this IServiceCollection services)
        {
            services.AddHealthChecks();
        }
        #endregion

        #region Data access
        public static void AddDataAccess(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            // SqlServer
            services.AddDbContext<SqlDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            // MongoDb
            var mongoDbSettingsSection = configuration.GetMongoDbSettingsSection();
            services.Configure<MongoDbSettings>(mongoDbSettingsSection);
            MongoDbContext.Configure();
            services.AddScoped<IMongoDbContext, MongoDbContext>();

            // UoW
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            //services.AddScoped<ITransactionalRepositoryManager, TransactionalRepositoryManager>();
        }
        #endregion

        #region Core services
        public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMapper, Mapper>();

            var jwtSettingsSection = configuration.GetJwtSettingsSection();
            services.Configure<JwtSettings>(jwtSettingsSection);
            services.AddScoped<IJwtProvider, JwtProvider>();

            services.AddScoped<IClockProvider, ClockProvider>();

            var googleAuthSettingsSection = configuration.GetGoogleAuthSettingsSection();
            services.Configure<GoogleAuthSettings>(googleAuthSettingsSection);
            services.AddScoped<IGoogleAuthProvider, GoogleAuthProvider>();

            var facebookAuthSettingsSection = configuration.GetFacebookAuthSettingsSection();
            services.Configure<FacebookAuthSettings>(facebookAuthSettingsSection);
            services.AddScoped<IFacebookAuthProvider, FacebookAuthProvider>();

            services.AddScoped<IPasswordProvider, PasswordProvider>();
        }
        #endregion

        #region Application service
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISecurityContextProvider, SecurityContextProvider>();
        }
        #endregion
    }
}
