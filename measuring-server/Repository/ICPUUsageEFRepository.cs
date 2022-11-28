using DataModel.EFDataModel;
using DataModel.MDDataModel;
using MeasuringServer.Model.Paging;
using System.Collections.Generic;


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
