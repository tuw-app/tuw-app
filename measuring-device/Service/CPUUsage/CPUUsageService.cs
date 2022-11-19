using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using MeasuringDevice.Model;

namespace MeasuringDevice.Service.CPUUsage
{
    public class UsageService : ICPUUsageService
    {
        private double CPUusegePercent=0;
        private CPUUsageResult usageResult;

        const string textCpu = "CPU Usage: ";
        private PerformanceCounter pc = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        private int interval = 1;
        
        public UsageService(int interval=1)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this.interval = interval;
            }
            else
                IncompatibleOS();
            
        }

        private void IncompatibleOS() => throw new CPUUsageExeption("CPU Usage only supported for Windows operating system.");


        public async Task<string> ReadCPUUsage()
        {
            pc.NextValue();
            await Task.Delay(interval);
            usageResult = new CPUUsageResult(pc.NextValue());
            return usageResult.GetShortString();

        }

        public override string ToString()
        {
            bool log = true;
            return GetCPUUsage(log);
        }

        public string GetCPUUsage(bool log=false)
        {
            if (log)
                return usageResult.ToString();
            else
                return usageResult.GetShortString();
        }
    }

}
