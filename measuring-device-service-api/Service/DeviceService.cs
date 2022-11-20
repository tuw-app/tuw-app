using Microsoft.Extensions.Logging;
using System;

namespace MeasuringDeviceServiceAPI.Service
{
    public class DeviceService: IDeviceService
    {
        private readonly ILogger logger;
        public Guid Id { get; }

        public DeviceService(ILogger<DeviceService> logger)
        {
            this.logger = logger;
            Id=Guid.NewGuid();
        }

        public void Write(string message)
        {
            logger.LogInformation("DeviceService -> Id: {id}", Id);
            logger.LogInformation("DeviceService -> Mesage:{message}", message);
        }

    }
}
