using MeasureFrontend.Services;
using Microsoft.AspNetCore.Components;

namespace MeasureFrontend.Components
{
    public partial class DeviceIntervalComponent : ComponentBase
    {
        [Inject]
        public IAPIMeasureDeviceService MDService { get; set; }
    }
}
