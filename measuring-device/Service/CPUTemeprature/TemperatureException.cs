using System;

namespace MeasureDeviceProject.Service.CPUTemeprature
{
    public class TemperatureException : Exception
    {
        public TemperatureException(string message)
            : base(message)
        { }
    }
}
