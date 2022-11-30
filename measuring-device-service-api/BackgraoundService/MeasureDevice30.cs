using DataModel.MDDataModel;
using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace MeasureDeviceServiceAPIProject.BackgraoundService
{
    public class MeasureDevice30 : MeasureDevice,IMeasureDevice30
    {
        private readonly ILogger<MeasureDevice> logger;
        private readonly IConfiguration configuration;

        public MeasureDevice30(IConfiguration configuration, ILogger<MeasureDevice> logger) :
            base(configuration,logger, 3, new MDIPAddress("30.30.30.30") , 3000)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public async void StartAsync()
        {
            CancellationToken token = new CancellationToken(false);

            if (token.IsCancellationRequested)
            {
                logger.LogInformation("ControllingContollers->Token starrt cancel is requested");
            }
            else
            {
                logger.LogInformation("ControllingContollers->Token stop cancel is not requested");
            }
            await StartAsync(token);
        }

        public async void StopAsync()
        {
            CancellationToken token = new CancellationToken(true);
            if (token.IsCancellationRequested)
            {
                logger.LogInformation("ControllingContollers->Token stop cancel is requested");
            }
            else
            {
                logger.LogInformation("ControllingContollers->Token stop cancel is not requested");
            }
            await StopAsync(token);
        }

        public void StopMeasuring()
        {
            StopMDMeasuring();
        }

        public void StartMeasuring()
        {
            StartMDMeasuring();
        }

        public void SetInterval(long ms)
        {
            MDMeasuringInterval = ms;
        }

        public MDState GetState()
        {
            return MDState;
        }

        public long GetInterval()
        {
            return MDMeasuringInterval;
        }
    }
}
