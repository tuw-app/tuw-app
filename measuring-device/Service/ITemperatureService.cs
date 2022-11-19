using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringDevice.Service
{
    public interface ITemperatureService
    {
        public string getTemperatureString();
        public List<TemperatureResult> getTemperatureResults();
    }
}
