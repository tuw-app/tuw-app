using MeasureDeviceProject.Model;
using MeasuringServer.Model;
using MeasuringServer.Model.Paging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeasuringServer.Repository
{
    public interface ICPUUsageEFRepository
    {
        List<EFCPUUsage> GetAllCPUUsage();
        EFCPUUsage GetCPUUsageById(MDDataId id);
        void CreateCPUUsage(EFCPUUsage cpuUsage);
        void UpdateCPUUsage(EFCPUUsage cpuUsage);
        void DeleteCPUUsage(EFCPUUsage cpuUsage);
        PagedList<EFCPUUsage> GetAllCPUUsageOfSpecificDevicePaged(string iPAddress, int page, int pagesize);
    }
}
