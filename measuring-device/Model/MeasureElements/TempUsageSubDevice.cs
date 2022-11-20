using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using MeasureDeviceProject.Service.CPUUsage;
using MeasureDeviceProject.Service.CPUTemeprature;


namespace MeasureDeviceProject.Model.MeasureElements
{
    public class TempUsageSubDevice : ITempUsageSubDevice
    {
        private readonly ILogger logger;
        private CPUUsageService usageService = new CPUUsageService();
        private CPUTemperatureService temperatureService = new CPUTemperatureService();

        public TempUsageSubDevice(ILogger<TempUsageSubDevice> logger)
        {
            this.logger = logger;
        }

        public string GetCurrentTemperature()
        {
            try
            {
                temperatureService.ReadTemperature();
                bool log = true;
                logger.Log(LogLevel.Information, temperatureService.GetTemperature(log));
                return temperatureService.GetTemperature(log = false);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                return string.Empty;
            }

        }

        public async Task<string> GetCurrentCpuUsage()
        {
            try
            {
                string result = await usageService.ReadCPUUsage();
                logger.Log(LogLevel.Information, usageService.ToString());
                return result;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                return string.Empty;
            }
        }
    }
}
