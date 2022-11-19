using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringDevice.Service.Temeprature
{
    public interface ITemperatureService
    {
        public string GetTemperature(bool log);
        public void ReadTemperature();
    }
}
