using MeasureDeviceProject.Model;
using MeasuringServer.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeasuringServer.Repository
{
    public interface ICPUUsageEFRepository
    {
        IEnumerable<CPUUsageEF> GetAllCPUUsage();
        CPUUsageEF GetCPUUsageById(MDDataId id);
        void CreateCPUUsage(CPUUsageEF cpuUsage);
        void UpdateCPUUsage(CPUUsageEF cpuUsage);
        void DeleteCPUUsage(CPUUsageEF cpuUsage);
    }
}
