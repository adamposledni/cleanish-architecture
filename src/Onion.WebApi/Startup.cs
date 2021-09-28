using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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
using System.Threading.Tasks;

namespace Onion.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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

            ConfigureSwagger(services);
            ConfigureCors(services);
            ConfigurePersistance(services);
            ConfigureLogic(services);
            ConfigureInfrastructure(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
