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
            services.AddHostedService<MeasureDevice>();
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
