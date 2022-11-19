using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringDevice.Service.Temeprature
{
    public class CPUTemperatureService : ITemperatureService
    {
        private OHMTemperatureService temperatureService=new OHMTemperatureService();

        public string GetTemperature(bool log)
        {
            return temperatureService.GetTemperature(log);
        }

        public void ReadTemperature()
        {
            temperatureService.ReadTemperature();
        }
    }
}
