
using DataModel.MDDataModel;
using MeasureFrontend.Services;

using Microsoft.AspNetCore.Components;


namespace MeasureFrontend.Components
{
    public partial class StatusComponent : ComponentBase
    {
        [Inject]
        public IAPIMeasureDeviceService MDService { get; set; }

        




    }
}
