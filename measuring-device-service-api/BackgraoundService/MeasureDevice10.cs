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
            base(configuration, logger, 1, new MDIPAddress("10.10.10.10"),5000)
        {
            this.logger = logger;
            this.configuration = configuration;

        }
    }
}
