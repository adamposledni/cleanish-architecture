using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Cleanish.App.Logic.Common.Security;
using Cleanish.Pres.WebApi.Security;
using Cleanish.Shared.Exceptions;
using System.Globalization;
using Cleanish.Impl.Shared.Security.WebToken;

namespace Cleanish.Pres.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        #region Controllers
        services.AddControllers()
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            })
            .ConfigureApiBehaviorOptions(opt =>
            {
                // handle model validation errors
                opt.InvalidModelStateResponseFactory = (ctx) =>
                {
                    var errorMessages = ctx.ModelState.Values
                    .Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage);

                    string errors = string.Join(" ", errorMessages);
                    throw new ValidationException(errors);
                };
            });
        #endregion

        #region Localization
        List<CultureInfo> supportedCultures = new()
        {
            new CultureInfo("en"),
            new CultureInfo("cs")
        };

        services.Configure<RequestLocalizationOptions>(opt =>
        {
            opt.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
            opt.SupportedCultures = supportedCultures;
            opt.SupportedUICultures = supportedCultures;
            opt.RequestCultureProviders = new[] { new AcceptLanguageHeaderRequestCultureProvider() };
            opt.ApplyCurrentCultureToResponseHeaders = true;
        });

        services.AddLocalization();
        #endregion

        #region Authentication
        services.AddScoped<ISecurityContextProvider, SecurityContextProvider>();

        var webTokenSettings = configuration.GetSection(WebTokenSettings.CONFIG_KEY).Get<WebTokenSettings>();
        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = webTokenSettings.GetSecurityKey(),
                    ValidIssuer = webTokenSettings.Issuer,
                    ValidAudience = webTokenSettings.Audience,
                    ClockSkew = TimeSpan.Zero,
                };
            });
        #endregion

        #region Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            //c.IncludeXmlComments(string.Format(@"{0}\Cleanish.WebApi.xml", System.AppDomain.CurrentDomain.BaseDirectory));
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Cleanish architecture API",
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
        #endregion

        #region Others
        services.AddHttpContextAccessor();

        services.AddCors(conf =>
        {
            conf.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
            });
        });

        services.AddHealthChecks();

        services.AddSpaStaticFiles(opt => opt.RootPath = "wwwroot");
        #endregion

        return services;
    }
}
