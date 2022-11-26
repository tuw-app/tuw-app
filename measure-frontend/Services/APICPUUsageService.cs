using MeasureDeviceProject.Model;
using MeasureDeviceServiceAPIProject.Model;
using MeasuringServer.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MeasuringServer.Model.Paging;

using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace MeasureFrontend.Services
{
    public class APICPUUsageService : IAPICPUUsageService
    {
        private readonly ILogger<APICPUUsageService> logger;
        private readonly HttpClient httpClient;

        public APICPUUsageService(ILogger<APICPUUsageService> logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }


        public async Task<PagedList<CPUUsageEF>> GetAllCPUUsages( MDIPAddress IPAddress, int page, int pageSize)
        {

            var response = await httpClient.GetAsync("CPUUsage/api/cpuusage/" + IPAddress.ToString() + "/" + page.ToString() + "/" + pageSize.ToString());

            var content = response.Content.ReadAsStringAsync();

            PagedList<CPUUsageEF> result = JsonConvert.DeserializeObject<PagedList<CPUUsageEF>>(content.Result);

            logger.LogInformation("CPUUsageService -> GetAllCPUUsages -> Gets CPU usages count: {Count}", result.Cout);
            
            return result;

        }
    }
}
