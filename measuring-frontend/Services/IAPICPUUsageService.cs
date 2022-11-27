using MeasureDeviceProject.Model;
using MeasureDeviceProject.Model;
using MeasuringServer.Model;
using MeasuringServer.Model.Paging;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeasureFrontend.Services
{
    public interface IAPICPUUsageService
    {
        Task<PagedList<EFCPUUsage>> GetAllCPUUsages(MDIPAddress IPAddress, int page, int pageSize);
    }
}
