using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

using MeasuringServer.Model;
using MeasuringServer.Model.Paging;
using MeasureFrontend.Services;
using MeasureDeviceProject.Model;

namespace MeasureFrontend.Components
{
    public partial class MeasureDeviceComponent : ComponentBase
    {
        [Inject]
        public IAPICPUUsageService CPUUsageService { get; set; }

        public PagedList<CPUUsageEF> CPUUsages { get; set; }

        protected async override Task OnInitializedAsync()
        {
            CPUUsages = await CPUUsageService.GetAllCPUUsages(new MDIPAddress("10.10.10.10"), 1, 30);
        }
    }
}
