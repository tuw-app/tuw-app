using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MeasureDeviceServiceAPIProject.BackgraoundService;
using MeasureDeviceServiceAPIProject.Service;
using MeasureDeviceProject.Model;
using Serilog;
using Microsoft.Extensions.Configuration;
using Serilog.Core;

namespace MeasureDeviceProject.BackgraoundService
{
    public abstract class MeasureDevice : BackgroundService, IMeasureDevice, IDisposable
    {
        private readonly ILogger<MeasureDevice> logger;
        private readonly IConfiguration configuration;

        public MDIPAddress IPAddress { get; set; }
        private MeasureSendingDataService msds;

        private double measuringInterval = 1000;
        public double MeasureingInterval
        {
            get { return measuringInterval; }
            set
            {
                measuringInterval = value;                
            }            
        }

        public MeasureDevice(IConfiguration configuration, ILogger<MeasureDevice> logger, MDIPAddress MDIPAddress, double measuringInterval)
        {
            this.logger = logger;
            IPAddress= MDIPAddress;
            this.measuringInterval = measuringInterval;

            msds = new MeasureSendingDataService(configuration, logger, IPAddress);
        }
        


        public void Start()
        {
            //logger.LogInformation("MeasureDevice {@IpAddress} -> Measuring Start", IPAddress);
        }

        public void Stop()
        {
            //logger.LogInformation("MeasureDevice {@IpAddress} -> Measuring Stop", IPAddress);
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine(IPAddress);
            logger.LogInformation("MeasureDevice {@IpAddress} -> StartAsync", IPAddress);
            logger.LogInformation("MeasureDevice {@IpAddress} -> StartAsync, mesuring interval is {Interval}", IPAddress, measuringInterval);

            Thread thredPeridodically = new Thread(new ThreadStart(msds.StoringDataPeriodically));
            //thredPeridodically.CurrentCulture
            thredPeridodically.Priority = ThreadPriority.Lowest;
            thredPeridodically.Start();
                            

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("MeasureDevice {@IpAddress} -> ExecuteAsync", IPAddress);

            while (!stoppingToken.IsCancellationRequested)
            {

                logger.LogInformation("MeasureDevice {@IpAddress}:  ExecuteAsync {time}", IPAddress, DateTimeOffset.Now.ToString("yyyy.MM.dd HH: mm:ss"));
                msds.MeasuringCPUUsage();
                await Task.Delay(TimeSpan.FromMilliseconds(measuringInterval), stoppingToken);

            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            //logger.LogInformation("MeasureDevice {@IpAddress} -> StopAsync: {time}", DateTimeOffset.Now);            
            return base.StopAsync(cancellationToken);
        }

        public void StartDevice()
        {

        }

        public void StopDevice()
        {
        }

        public override void Dispose()
        {
            if (msds != null)
            {
                msds.Dispose();
            }
            base.Dispose();
        }
    }
}
