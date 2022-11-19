using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringDevice.Model
{
    public class TemperatureResult
    {
        public byte Id { get; set; }
        public double CurrentValue { get; set; }
        public string InstanceName { get; set; }

        public string RoundedValue
        {
            get
            {
                double roundedValue=Math.Round(CurrentValue,2);
                return roundedValue.ToString();
            }
        }

        public string GetShorString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Id).Append(";").Append(RoundedValue).Append(";");
            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CPU temperature->Id: ")
              .Append(Id)
              .Append("CPU name: ")
              .Append(InstanceName)
              .Append("temperature: ")
              .Append(CurrentValue)
              .Append("celsius, reounded temperature: ")
              .Append(RoundedValue)
              .Append(" celsius.");
            return sb.ToString();
        }
    }
}
