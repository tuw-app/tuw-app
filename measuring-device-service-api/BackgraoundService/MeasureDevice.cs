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
    public class MeasureDevice : BackgroundService, IMeasureDevice
    {
        public MDIPAddress IPAddress { get; set; }

        private readonly ILogger<MeasureDevice> logger;
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

        public MeasureDevice(ILogger<MeasureDevice> logger)
        {
            //IPAddress = new MDIPAddress("1.1.1.1");
            //string logFileName = @"d:\tuw\log\log" + IPAddress + ".log";

            this.logger = logger;

            this.IPAddress = new MDIPAddress("1.1.1.1");
            //msds = new MeasureSendingDataService(MeasureingInterval);           
            logger.LogInformation("MeasureDevice {@IpAddress} -> Created", IPAddress);
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
