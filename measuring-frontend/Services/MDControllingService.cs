using DataModel.EFDataModel;
using MeasuringServer.Model.Paging;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MeasureFrontend.Services
{
    public class MDControllingService : IMDControllingService
    {
        private readonly ILogger<MDControllingService> logger;
        private readonly HttpClient httpClient;

        public MDControllingService(ILogger<MDControllingService> logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        public async Task<bool> ControlDevice(string controlCommand, string IPAddress)
        {
            try
            {
                var response = await httpClient.GetAsync($"/api/{controlCommand}/{IPAddress}");

                var content = response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
