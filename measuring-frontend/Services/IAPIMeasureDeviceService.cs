using DataModel.EFDataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeasureFrontend.Services
{
    public interface IAPIMeasureDeviceService
    {
        Task<List<EFMeasureDevice>> GetAllMeasureDevices();
    }
}
