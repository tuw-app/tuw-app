using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringDevice.Service
{
    public class TemperatureResult 
    {
        public double CurrentValue { get; set; }
        public string InstanceName { get; set; }

        public override string ToString()
        {
            string result = $"Instance name: {InstanceName} , temperature: {CurrentValue.ToString()} celsius";
            return result;
        }
    }
}
