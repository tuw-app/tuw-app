using DataModel.MDDataModel;
using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace MeasureDeviceServiceAPIProject.BackgraoundService
{
    public class MeasureDevice20 : MeasureDevice, IMeasureDevice20
    {
        private readonly ILogger<MeasureDevice> logger;
        private readonly IConfiguration configuration;

        public MeasureDevice20(IConfiguration configuration, ILogger<MeasureDevice> logger) :
            base(configuration, logger, new MDIPAddress("20.20.20.20"),2000)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public void Start()
        {

        }

        public void StartMeasuring()
        {

        }

        public void Stop()
        {

        }

        public void StopMeasuring()
        {

        }
    }
}
