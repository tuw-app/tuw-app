using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using Microsoft.Extensions.Logging;
using System;

namespace MeasureDeviceServiceAPIProject.BackgraoundService
{
    public class MeasureDevice30 : MeasureDevice
    {
        private readonly ILogger<MeasureDevice> logger;

        public MeasureDevice30(ILogger<MeasureDevice> logger) :
            base(logger, new MDIPAddress("30.30.30.30"),3000)
        {
            this.logger = logger;
        }
    }
}
