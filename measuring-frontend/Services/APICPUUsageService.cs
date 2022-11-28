using Newtonsoft.Json;

using System.Net.Http;
using System.Threading.Tasks;
using MeasuringServer.Model.Paging;

using Microsoft.Extensions.Logging;
using DataModel.EFDataModel;
using DataModel.MDDataModel;

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


        public async Task<PagedList<EFCPUUsage>> GetAllCPUUsages( MDIPAddress IPAddress, int page, int pageSize)
        {

            var response = await httpClient.GetAsync("/api/cpuusage/" + IPAddress.ToString() + "/" + page.ToString() + "/" + pageSize.ToString());

            var content = response.Content.ReadAsStringAsync();

            PagedList<EFCPUUsage> result = JsonConvert.DeserializeObject<PagedList<EFCPUUsage>>(content.Result);

            if (result != null)
            {
                logger.LogInformation("CPUUsageService -> GetAllCPUUsages -> Gets CPU usages count: {Count}", result.Count);
                return result;
            }
            else
            {
                logger.LogInformation("CPUUsageService -> GetAllCPUUsages -> No CPU usages");
                return new PagedList<EFCPUUsage>() ;
            }

        }
    }
}
