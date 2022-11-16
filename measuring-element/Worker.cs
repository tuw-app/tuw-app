using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TUWWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;

        public Worker(ILogger<Worker> logger)
        {
            this.logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.Log(LogLevel.Information, "Worker StartAsync");
            await base.StartAsync(cancellationToken);
            logger.Log(LogLevel.Information, "Worker StartAsync elindult");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.Log(LogLevel.Information, "Worker StopAsync");
            await base.StopAsync(cancellationToken);
            logger.Log(LogLevel.Information, "Worker StopAsync leált");
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override void Dispose()
        {
        }
    }
}
