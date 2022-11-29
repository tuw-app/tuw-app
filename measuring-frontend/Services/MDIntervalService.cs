using DataModel.MDDataModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MeasureFrontend.Services
{
    public class MDIntervalService : IMDIntervalService
    {
        private readonly ILogger<MDIntervalService> logger;
        private readonly HttpClient httpClient;

        public MDIntervalService(ILogger<MDIntervalService> logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        public async Task<long> GetMDInterval(string IPAddress)
        {
            try
            {
                var response = await httpClient.GetAsync("/api/interval/"+IPAddress.ToString());

                var content = response.Content.ReadAsStringAsync();

                long result = JsonConvert.DeserializeObject<long>(content.Result);

                if (result != null)
                {
                    logger.LogInformation("MDIntervalService -> GetMDInterval -> Gets interval: {IPAddress} -> {interval}", IPAddress, result);
                    return result;
                }
                else
                {
                    logger.LogInformation("MDIntervalService -> GetMdSatates -> No statates: {IPAddress}", IPAddress);
                    return -1;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return -1;
            }
        }
    }
}
