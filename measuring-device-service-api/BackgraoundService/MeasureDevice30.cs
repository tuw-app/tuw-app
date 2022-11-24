using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace MeasureDeviceServiceAPIProject.BackgraoundService
{
    public class MeasureDevice30 : MeasureDevice
    {
        private readonly ILogger<MeasureDevice> logger;
        private readonly IConfiguration configuration;

        public MeasureDevice30(IConfiguration configuration, ILogger<MeasureDevice> logger) :
            base(configuration,logger, new MDIPAddress("30.30.30.30"),3000)
        {
            this.logger = logger;
            this.configuration = configuration;
        }
    }
}
