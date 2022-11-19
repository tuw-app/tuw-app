using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringDevice.Service.CPUUsage
{
    public interface ICPUUsageService
    {
        public string GetCPUUsage(bool log);
        //public async void ReadCPUUsage();
    }
}
