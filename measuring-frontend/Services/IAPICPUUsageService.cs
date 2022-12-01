using DataModel.EFDataModel;
using DataModel.MDDataModel;

using MeasuringServer.Model.Paging;
using System;
using System.Threading.Tasks;

namespace MeasureFrontend.Services
{
    public interface IAPICPUUsageService
    {
        Task<PagedList<EFCPUUsage>> GetAllCPUUsages(MDIPAddress IPAddress, DateTime startDate, DateTime endDate, int page, int pageSize);
    }
}
