using MeasuringDeviceAPI.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeasuringDeviceAPI.Services
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IServiceProvider serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider deviceService)
        {
            this.logger = logger;
            this.serviceProvider = deviceService;
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
                using (var scope = serviceProvider.CreateScope())
                {
                    logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    var deviceService=scope.ServiceProvider.GetService<IDeviceService>();
                    deviceService.Write("I am a CPU temperature.");
                    deviceService.Write("I am a CPU usage.");
                    await Task.Delay(TimeSpan.FromMilliseconds(1000), stoppingToken);
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("From BackgroundService -> StopAsync: {time}",DateTimeOffset.Now);
            return base.StopAsync(cancellationToken);
        }
    }
}
