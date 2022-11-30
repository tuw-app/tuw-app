using DataModel.MDDataModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

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
                var response = await httpClient.GetAsync("/api/interval/" + IPAddress.ToString());

                var content = response.Content.ReadAsStringAsync();

                long result = JsonConvert.DeserializeObject<long>(content.Result);

                if (result > 0 )
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

        public async Task<bool> SendIntervalToDevice(string IPAddress, MDIntervalData interval)
        {
            //Uri u = new Uri("http://localhost:5001/api/cpuusage");
            UriBuilder u = new UriBuilder();
            u.Scheme = "http";
            u.Host = "localhost";
            u.Path = "/api/interval/" + IPAddress + "/";
            u.Port = 5000;

            logger.LogInformation("MDIntervalService -> SendIntervalToDevice -> Uri is {Uri}", u.ToString());

            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = u.Uri;

                    logger.LogInformation("MDIntervalService -> SendIntervalToDevice data -> {data}", interval);

                    String jsonString = JsonConvert.SerializeObject(interval);
                    StringContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(u.Uri, httpContent);

                    if (response != null)
                    {
                        logger.LogInformation("MDIntervalService -> SendIntervalToDevice-> Status code: {Code}", response.StatusCode);
                        if (response.StatusCode == HttpStatusCode.OK)
                            return true;
                        else
                        {
                            string error = response.Headers + " : " + response.Content + " : " + response.StatusCode;
                            logger.LogInformation("MDIntervalService -> SendIntervalToDevice-> Error {error}", error.ToString());
                            return false;
                        }
                    }
                    else
                    {
                        logger.LogInformation("MDIntervalService -> SendIntervalToDevice-> Response is null");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogInformation("MDIntervalService -> SendIntervalToDevice-> Exception {Message}", ex.Message);
                    return false;
                }
            }

        }
    }
}