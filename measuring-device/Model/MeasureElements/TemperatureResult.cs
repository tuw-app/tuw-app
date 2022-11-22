using System;
using System.Text;

namespace MeasureDeviceProject.Model.MeasureElements
{
    public class TemperatureResult : IResult
    {
        public byte Id { get; set; }
        public double CurrentValue { get; set; }
        public string InstanceName { get; set; }

        public string RoundedValue
        {
            get
            {
                double roundedValue = Math.Round(CurrentValue, 2);
                return roundedValue.ToString();
            }
        }

        public string GetShortString()
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
              .Append(" CPU name: ")
              .Append(InstanceName)
              .Append(" temperature: ")
              .Append(CurrentValue)
              .Append(" celsius, reounded temperature: ")
              .Append(RoundedValue)
              .Append(" celsius.");
            return sb.ToString();
        }
    }
}
