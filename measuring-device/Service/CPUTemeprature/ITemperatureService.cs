using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringDevice.Service.CPUTemeprature
{
    public interface ITemperatureService
    {
        public string GetTemperature(bool log);
        public void ReadTemperature();
    }
}
