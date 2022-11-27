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

     /*   public PagedList<EFCPUUsage> CPUUsages { get; set; }

        [Parameter]
        public string? IPAddress { get; set; }

        protected async override Task OnInitializedAsync()
        {
            CPUUsages = await CPUUsageService.GetAllCPUUsages(new MDIPAddress(IPAddress), 1, 30);
        }*/
    }
}
