using System.Diagnostics;
using System.Runtime.InteropServices;
using MeasureDeviceProject.Model.MeasureElements;
using System;
using System.Threading.Tasks;

namespace MeasureDeviceProject.Service.CPUUsage
{
    public class CPUUsageService : ICPUUsageService, IDisposable
    {
        private CPUUsageResult usageResult;

        const string textCpu = "CPU Usage: ";
        private PerformanceCounter pc;

        private int interval = 1;
        
        public CPUUsageService(int interval=1)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this.interval = interval;                
                pc = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            }
            else
                IncompatibleOS();
            
        }

        private void IncompatibleOS()
        {
            //=> throw new CPUUsageExeption("CPU Usage only supported for Windows operating system.");
            usageResult = new CPUUsageResult();
        }

        public async Task<string> ReadCPUUsage()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {                    
                    pc.NextValue();
                    await Task.Delay(interval);
                    usageResult = new CPUUsageResult(pc.NextValue());
                    return usageResult.GetShortString();
                }
                catch
                {
                    usageResult = new CPUUsageResult();
                    return string.Empty;
                } 
                
            }
            else
            {
                return string.Empty;
            }
        }

        public override string ToString()
        {
            bool log = true;
            return GetCPUUsage(log);
        }

        public string GetCPUUsage(bool log=false)
        {
            if (usageResult != null)
            {
                if (log)
                    return usageResult.ToString();
                else
                    return usageResult.GetShortString();
            }
            else
                return string.Empty;
        }

        public string GetCPUUsageToLog()
        {
            bool log = true;
            return GetCPUUsage(log);
        }

        public void Dispose()
        {
            if (pc != null)
                pc.Dispose();
        }
    }

}
