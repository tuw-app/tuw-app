using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MeasureDeviceProject.Model;
using MeasureDeviceServiceAPIProject.BackgraoundService;
using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceServiceAPIProject.Service;

namespace MeasureDeviceServiceAPIProject
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
            services.AddControllers();

            //services.AddScoped<IDeviceService, DeviceService>();

            // A mérõeszközt és az adattovábbító rendszert is tartalmazó mérõeszköz
            //services.AddScoped<IMeasureDevice, MeasureDevice>();

            // Az összes mérõeszközt tartalmazza
            //services.AddHostedService<MEFactory>();
            // https://stackoverflow.com/questions/58397807/how-to-resolve-hostedservice-in-controller
            // https://medium.com/medialesson/run-and-manage-periodic-background-tasks-in-asp-net-core-6-with-c-578a31f4b7a3

            // https://stackoverflow.com/questions/51254053/how-to-inject-a-reference-to-a-specific-ihostedservice-implementation
            services.AddSingleton< IMeasureDevice10, MeasureDevice10>();
            services.AddSingleton<IMeasureDevice20, MeasureDevice20>();
            services.AddSingleton<IMeasureDevice30, MeasureDevice30>();
            services.AddSingleton<IHostedService, MeasureDevice10>(provider =>(MeasureDevice10) provider.GetService<IMeasureDevice10>());
            services.AddSingleton<IHostedService, MeasureDevice20>(provider =>(MeasureDevice20) provider.GetService<IMeasureDevice20>());
            services.AddSingleton<IHostedService, MeasureDevice30>(provider => (MeasureDevice30)provider.GetService<IMeasureDevice20>());
            //services.AddHostedService<MeasureDevice20>();
            //services.AddHostedService<MeasureDevice30>(provider => provider.GetService<MeasureDevice30>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
