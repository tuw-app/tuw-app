using MeasureDeviceProject.Model;
using MeasuringServer.Model.Paging;
using MeasuringServer.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System;
using DataModel.EFDataModel;

namespace MeasureFrontend.Services
{
    public class APIMeasureDeviceService : IAPIMeasureDeviceService
    {
        private readonly ILogger<APICPUUsageService> logger;
        private readonly HttpClient httpClient;

        public APIMeasureDeviceService(ILogger<APICPUUsageService> logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        public async Task<List<EFMeasureDevice>> GetAllMeasureDevices()
        {
            try
            {
                var response = await httpClient.GetAsync("MeasureDevice/api/md");

                var content = response.Content.ReadAsStringAsync();
                List<EFMeasureDevice>  devices = JsonConvert.DeserializeObject<List<EFMeasureDevice>>(content.Result);

                logger.LogInformation("CPUUsageService -> GetAllCPUUsages -> Gets CPU usages count: {Count}", devices.Count);
                return devices;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return new List<EFMeasureDevice>();
            }          
        }
    }
}
