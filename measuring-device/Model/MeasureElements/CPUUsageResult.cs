using System;
using System.Text;


namespace MeasureDeviceProject.Model.MeasureElements
{
    public class CPUUsageResult : IResult
    {
        public double CPUUsage { get; set; }

        public double RoundedCPUUsage
        {
            get
            {
                if (CPUUsage != double.NaN)
                {
                    double roundedCPUUsage = Math.Round(CPUUsage, 2);
                    return roundedCPUUsage;
                }
                else
                    return double.NaN;
            }
        }

        public CPUUsageResult()
        {
            CPUUsage = double.NaN;
        }

        public CPUUsageResult(double cpuUsage)
        {
            CPUUsage = cpuUsage;
        }

        public string GetShortString()
        {
            if (CPUUsage == double.NaN)
                return string.Empty;
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(RoundedCPUUsage).Append(";");
                return sb.ToString();
            }
        }

        public override string ToString()
        {
            if (CPUUsage == double.NaN)
                return string.Empty;
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("CPU usage-> ")
                  .Append(RoundedCPUUsage)
                  .Append("%.");
                return sb.ToString();
            }
        }
    }
}
