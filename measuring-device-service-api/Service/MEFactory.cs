using MeasureDeviceServiceAPIProject.BackgraoundService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MeasureDeviceProject.Model;
using System.Linq;

namespace MeasureDeviceServiceAPIProject.Service
{
    public class MEFactory : BackgroundService
    {
        private readonly ILogger<MEFactory> logger;
        private readonly IServiceProvider serviceProvider;

        List<IMeasureDevice> devices = new List<IMeasureDevice>();

        public MEFactory(ILogger<MEFactory> logger, IServiceProvider deviceService)
        {
            this.logger = logger;
            serviceProvider = deviceService;
            /*
            for (int i = 0; i < 1; i++)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<IMeasureDevice>();
                    service.IPAddress = new MDIPAddress("1.1.1.1");
                    logger.LogInformation("MEFactory -> New Mesure device with IP address { address } created", service.IPAddress);
                    devices.Add(service);
                }
            }
            */
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("MEFactory -> StartAsync");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("MEFactory -> ExecuteAsync");
            while (!stoppingToken.IsCancellationRequested)
            {

                // Ide a vezérlés kezelés kell rakni!
                logger.LogInformation("MEFactory running at: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromMilliseconds(1000), stoppingToken);

             }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("MEFactory -> StopAsync: {time}", DateTimeOffset.Now);
           
            return base.StopAsync(cancellationToken);
        }
    }
}
