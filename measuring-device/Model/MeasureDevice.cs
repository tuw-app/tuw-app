using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MeasuringDevice.Service.CPUUsage;
using MeasuringDevice.Service.Temeprature;

namespace MeasuringDevice.Model
{
    public class MeasureDevice
    {
        public IPAddress IPAddress { get; set; }
        public int MeasuringPeriod { get; set; }

        private CPUUsageService usageService = new CPUUsageService();
        private CPUTemperatureService temperatureService= new CPUTemperatureService();

        public MeasureDevice(IPAddress iPAddress, int measuringPeriod)
        {
            IPAddress = iPAddress;
            MeasuringPeriod = measuringPeriod;
        }




    }
}
