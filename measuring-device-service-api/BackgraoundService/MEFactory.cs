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

namespace MeasureDeviceServiceAPIProject.Services
{
    public class MEFactory : BackgroundService
    {
        private readonly ILogger<MEFactory> logger;
        private readonly IServiceProvider serviceProvider;

        List<IDeviceService> devices = new List<IDeviceService>();

        public MEFactory(ILogger<MEFactory> logger, IServiceProvider deviceService)
        {
            this.logger = logger;
            this.serviceProvider = deviceService;

            for (int i = 0; i < 1; i++)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<IDeviceService>();
                    service.IPAddress = new MDIPAddress("1.1.1.1");
                    logger.LogInformation("New Mesure device with IP address { address } created",service.IPAddress);
                    devices.Add(service);
                }
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("From BackgroundService -> StartAsync");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("From BackgroundService -> ExecuteAsync");
            while (!stoppingToken.IsCancellationRequested)
            {

                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                devices.ElementAt(0).Start();
                await Task.Delay(TimeSpan.FromMilliseconds(1000), stoppingToken);

            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("From BackgroundService -> StopAsync: {time}",DateTimeOffset.Now);
            devices.ElementAt(0).Stop();
            return base.StopAsync(cancellationToken);
        }
    }
}
