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

namespace MeasureDeviceProject.BackgraoundService
{
    public class MeasureDevice : BackgroundService, IMeasureDevice
    {
        public MDIPAddress IPAddress { get; set; }

        private readonly ILogger<MEFactory> logger;
        private MeasurSendingDataService msds;

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

        public MeasureDevice()
        {
            msds = new MeasurSendingDataService(logger, MeasureingInterval);
            
            //logger.LogInformation("MeasureDevice { IpAddress } -> Created", IPAddress);
        }

        public void Start()
        {
            //logger.LogInformation("MeasureDevice { IpAddress } -> Started", IPAddress);
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
