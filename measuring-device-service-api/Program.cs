using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace MeasureDeviceServiceAPIProject
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            var path = configuration.GetValue<string>("LogMeasurePath");

            const string logTemplate = @"{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u4}] [{SourceContext:l}] {Message:lj}{NewLine}{Exception}";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .WriteTo.Console()
                .WriteTo.Debug()
                .WriteTo.File(path+"log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Logger(l =>
                {
                    l.WriteTo.File(path + "log-10.10.10.10-.txt", LogEventLevel.Information, logTemplate,
                        rollingInterval: RollingInterval.Day
                    );
                    l.Filter.ByIncludingOnly(e => e.Properties.ContainsKey("10.10.10.10"));
                })
                .WriteTo.Logger(l =>
                {
                    l.WriteTo.File(path + "log-20.20.20.20-.txt", LogEventLevel.Information, logTemplate,
                        rollingInterval: RollingInterval.Day
                    );
                    l.Filter.ByIncludingOnly(e => e.Properties.ContainsKey("20.20.20.20"));
                })
                .WriteTo.Logger(l =>
                {
                    l.WriteTo.File(path + "log-30.30.30.30-.txt", LogEventLevel.Information, logTemplate,
                        rollingInterval: RollingInterval.Day
                    );
                    l.Filter.ByIncludingOnly(e => e.Properties.ContainsKey("30.30.30.30"));
                })
                .CreateLogger();

            try
            {
                Log.Information("measuring-system started");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                Log.Fatal("measuring-system stopped unexpectedly");
                Log.Fatal(exception.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
