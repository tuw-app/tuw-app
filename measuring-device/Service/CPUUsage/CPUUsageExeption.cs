using System;

namespace MeasureDeviceProject.Service.CPUUsage
{
    public class CPUUsageExeption : Exception
    {
        public CPUUsageExeption(string message) : base(message)
        {
        }
    }
}
