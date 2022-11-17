using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Formatting.Compact;


using System;
using System.Globalization;

using LoggerLibrary.CustomDateFormatterLib;
using Serilog.Templates;

namespace TUWWorker
{

    /*
     .WriteTo.Console(new RenderedCompactJsonFormatter())
    .WriteTo.Console(formatProvider: formatter)
    .WriteTo.Debug(formatProvider: formatter)
    .WriteTo.Console(new ExpressionTemplate("[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"), Serilog.Events.LogEventLevel.Debug)
    .WriteTo.Console(new ExpressionTemplate("[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"), Serilog.Events.LogEventLevel.Debug)
    .WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
    .WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
    .WriteTo.File(new ExpressionTemplate("[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"), @"d:\temp\log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Debug(new ExpressionTemplate("[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
    .WriteTo.File(@"d:\temp\log.txt", rollingInterval: RollingInterval.Day)
    */


    /*
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Debug)
                .Enrich.FromLogContext()
                .WriteTo.Debug(formatProvider: formatter)
                .WriteTo.File(@"d:\temp\log.txt", rollingInterval: RollingInterval.Day))
    */
    public class Program
    {
        public static void Main(string[] args)
        {


            // , new RenderedCompactJsonFormatter(), 
            var formatter = new CustomDateFormatter("yyyy.MM.dd", new CultureInfo("hu-Hu"));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Debug(formatProvider: formatter)
                .WriteTo.File(@"d:\temp\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            try
            {
                Log.Information("measuring-element indul");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
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
                    .ConfigureLogging(loggerFactory =>
                    {
                        loggerFactory.SetMinimumLevel(LogLevel.Debug);
                        loggerFactory.AddFile(@"f:\temp\log-{Date}.json", isJson: true);                        
                        loggerFactory.AddEventLog();

                    })                        
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
