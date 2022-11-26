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
        Task<PagedList<CPUUsageEF>> GetAllCPUUsages(MDIPAddress IPAddress, int page, int pageSize);
    }
}
