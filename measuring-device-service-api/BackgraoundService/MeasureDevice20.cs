using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using Microsoft.Extensions.Logging;
using System;

namespace MeasureDeviceServiceAPIProject.BackgraoundService
{
    public class MeasureDevice20 : MeasureDevice
    {
        private readonly ILogger<MeasureDevice> logger;

        public MeasureDevice20(ILogger<MeasureDevice> logger) :
            base(logger, new MDIPAddress("20.20.20.20"),2000)
        {
            this.logger = logger;
        }
    }
}
