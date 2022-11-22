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

namespace MeasureDeviceProject.BackgraoundService
{
    public abstract class MeasureDevice : BackgroundService, IMeasureDevice
    {
        private readonly ILogger<MeasureDevice> logger;

        public MDIPAddress IPAddress { get; set; }
        private MeasureSendingDataService msds;

        private double measuringInterval = 1000;
        public double MeasureingInterval
        {
            get { return measuringInterval; }
            set
            {
                measuringInterval = value;
                msds.SetMeasureingInterval(value);
            }            
        }

        public MeasureDevice(ILogger<MeasureDevice> logger, MDIPAddress MDIPAddress, double measuringInterval)
        {
            this.logger = logger;
            IPAddress= MDIPAddress;
            this.measuringInterval = measuringInterval;
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
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("MeasureDevice {@IpAddress} -> ExecuteAsync", IPAddress);
            while (!stoppingToken.IsCancellationRequested)
            {

                // Ide a vezérlés kezelés kell rakni!
                logger.LogInformation("MeasureDevice {@IpAddress} -> ExecuteAsync", IPAddress);
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
    }
}
