using MeasureFrontend.Data;
using MeasureFrontend.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeasureFrontend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddHttpClient<IAPICPUUsageService, APICPUUsageService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5001/");
            });

            services.AddHttpClient<IAPIMeasureDeviceService, APIMeasureDeviceService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5001/");
            });

            services.AddHttpClient<IMDStateService, MDStateService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000/");
            });

            services.AddHttpClient<IMDControllingService, MDControllingService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000/");
            });
            services.AddHttpClient<IMDIntervalService, MDIntervalService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000/");
            });
            

            //services.AddSingleton<WeatherForecastService>();
            //services.AddSingleton<APICPUUsageService>();
            //services.AddSingleton< APIMeasureDeviceService()>;

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
        
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
