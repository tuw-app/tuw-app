using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using Microsoft.Extensions.Logging;
using System;

namespace MeasureDeviceServiceAPIProject.BackgraoundService
{
    public class MeasureDevice10 : MeasureDevice
    {
        private readonly ILogger<MeasureDevice> logger;

        public MeasureDevice10(ILogger<MeasureDevice> logger) :
            base(logger, new MDIPAddress("10.10.10.10"),1000)
        {
            this.logger = logger;
        }
    }
}
