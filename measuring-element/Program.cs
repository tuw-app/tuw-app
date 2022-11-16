using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;
using System;

namespace TUWWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"f:\temp\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            try
            {
                Log.Information("measuring-element indul");
                CreateHostBuilder(args).Build().Run();
            }
            catch(Exception exception)
            {
                Log.Fatal("measuring-element váratlanul leált");
                Log.Debug(exception.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                        webBuilder.UseUrls("http://localhost:5010");
                    })
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddHostedService<Worker>();
                    });
    }
}
