using DataModel.MDDataModel;
using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace MeasureDeviceServiceAPIProject.BackgraoundService
{
    public class MeasureDevice10 : MeasureDevice, IMeasureDevice10
    {
        private readonly ILogger<MeasureDevice> logger;
        private readonly IConfiguration configuration;

        public MeasureDevice10(IConfiguration configuration, ILogger<MeasureDevice> logger) :
            base(configuration, logger, new MDIPAddress("10.10.10.10"),1000)
        {
            this.logger = logger;
            this.configuration = configuration;

        }

        public async void Start()
        {
            logger.LogInformation("10.10.10.10->Start");
            await this.StartAsync(cancellationToken: default);
        }

        

        public async void Stop()
        {
            logger.LogInformation("10.10.10.10->Stop");
            await this.StartAsync(cancellationToken: default);
        }

        public void StartMeasuring()
        {
        }

        public void StopMeasuring()
        {
        }
    }
}
