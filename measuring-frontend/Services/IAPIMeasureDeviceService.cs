using MeasureDeviceProject.Model;
using MeasuringServer.Model;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeasureFrontend.Services
{
    public interface IAPIMeasureDeviceService
    {
        Task<List<EFMeasureDevice>> GetAllMeasureDevices();
    }
}
