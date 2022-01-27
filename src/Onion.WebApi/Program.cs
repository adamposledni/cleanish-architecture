using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Onion.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((ctx, conf) =>
        {
            conf.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            conf.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
            conf.AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", optional: true);
            conf.AddEnvironmentVariables();
        })
        .ConfigureLogging(builder =>
        {
        })
        .ConfigureWebHostDefaults(builder =>
        {
            builder.UseStartup<Startup>();
        });
}
