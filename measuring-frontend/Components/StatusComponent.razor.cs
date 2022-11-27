using MeasureDeviceProject.Model;
using MeasureFrontend.Services;
using MeasuringServer.Model.Paging;
using MeasuringServer.Model;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System;

namespace MeasureFrontend.Components
{
    public partial class StatusComponent : ComponentBase
    {
        [Inject]
        public IAPIMeasureDeviceService MDService { get; set; }

/*        private List<EFMeasureDevice> devices;


        //private readonly ILogger<APICPUUsageService> logger;


        protected async override Task OnInitializedAsync()
        {
            try
            {
                devices = await MDService.GetAllMeasureDevices();
                if (devices != null)
                    System.Console.WriteLine("StatusComponent-> Number of md: {Count}", devices.Count);
                else
                {
                    devices = new List<EFMeasureDevice>();
                    System.Console.WriteLine("StatusComponent-> No devices in database;");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            
        }
*/
        

    }
}
