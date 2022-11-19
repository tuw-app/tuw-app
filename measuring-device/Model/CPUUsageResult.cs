using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringDevice.Model
{
    public class CPUUsageResult : IResult
    {
        public double CPUUsage { get; set; }

        public double RoundedCPUUsage
        {
            get             
            { 
                double roundedCPUUsage=Math.Round(CPUUsage, 2);
                return roundedCPUUsage; 
            }
        }


        public CPUUsageResult(double cpuUsage)
        {
            CPUUsage = cpuUsage;
        }

        

        public string GetShortString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(RoundedCPUUsage).Append(";");
            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CPU usage-> ")
              .Append(RoundedCPUUsage)
              .Append("%.");
            return sb.ToString();
        }
    }
}
