using DataModel.EFDataModel;
using DataModel.MDDataModel;
using MeasuringServer.Model.Paging;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MeasureFrontend.Services
{
    public class MDStateService : IMDStateService
    {

        private readonly ILogger<MDStateService> logger;
        private readonly HttpClient httpClient;

        public MDStateService(ILogger<MDStateService> logger, HttpClient httpClient) 
        { 
            this.logger = logger;
            this.httpClient = httpClient;
        }

        public async Task<MDState> GetMdSatates(string IPAddress)
        {
            try
            {
                var response = await httpClient.GetAsync("/api/" + IPAddress.ToString() + "/state");

                var content = response.Content.ReadAsStringAsync();

                MDState result = JsonConvert.DeserializeObject<MDState>(content.Result);

                if (result != null)
                {
                    logger.LogInformation("MDStateService -> GetMdSatates -> Gets statates: {IPAddress} -> {States}", IPAddress, result);
                    return result;
                }
                else
                {
                    logger.LogInformation("MDStateService -> GetMdSatates -> No statates: {IPAddress}", IPAddress);
                    return new MDState();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new MDState();
            }
        }


    }
}
