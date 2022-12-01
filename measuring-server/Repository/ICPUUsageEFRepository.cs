using DataModel.EFDataModel;
using DataModel.MDDataModel;
using MeasuringServer.Model.Paging;
using System;
using System.Collections.Generic;


namespace MeasuringServer.Repository
{
    public interface ICPUUsageEFRepository
    {
        List<EFCPUUsage> GetAllCPUUsage(string IPAddress, DateTime startTime, DateTime endTime);
        EFCPUUsage GetCPUUsageById(MDDataId id);
        void CreateCPUUsage(EFCPUUsage cpuUsage);
        void UpdateCPUUsage(EFCPUUsage cpuUsage);
        void DeleteCPUUsage(EFCPUUsage cpuUsage);
        PagedList<EFCPUUsage> GetAllCPUUsageOfSpecificDevicePaged(string iPAddress, DateTime startTime, DateTime endTime, int page, int pagesize);
    }
}
