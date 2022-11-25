using System;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MeasureDeviceProject.BackgraoundService;
using Microsoft.Extensions.Logging;

namespace MeasureDeviceServiceAPIProject.APIService
{
    public class CPUAPIService
    {

        ILogger<MeasureDevice> logger = null;

        public CPUAPIService(ILogger<MeasureDevice> logger)
        {
            this.logger= logger;
        }

        public async Task<HttpStatusCode> SendNewCPUDataAsync(string CPUdata)
        {
            //Uri u = new Uri("http://localhost:5001/api/cpuusage");
            UriBuilder u = new UriBuilder();
            u.Scheme= "http";
            u.Host = "localhost";
            u.Path = "/api/cpuusage";
            u.Port = 5001;

            logger.LogInformation("CPUAPIService -> Uri is {Uri}", u.ToString());

            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = u.Uri;
                    String jsonString = JsonConvert.SerializeObject(CPUdata);
                    StringContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    logger.LogInformation("CPUAPIService -> StringContent-> {Content}", httpContent.ToString());

                    var response = await httpClient.PostAsync("/Subject/api/subject", httpContent);

                    if (response != null)
                    {
                        logger.LogInformation("CPUAPIService -> StringContent-> {Response}", response.ToString());
                        if (response.StatusCode == HttpStatusCode.Created)
                            return HttpStatusCode.OK;
                        else
                        {

                            string error = response.Headers + " : " + response.Content + " : " + response.StatusCode;
                            logger.LogInformation("CPUAPIService -> StringContent-> {Response}", error.ToString());
                            return HttpStatusCode.InternalServerError;
                        }
                    }
                    else
                    {
                        logger.LogInformation("CPUAPIService -> StringContent-> Response is null");
                        return HttpStatusCode.InternalServerError;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogInformation("CPUAPIService -> StringContent-> Exception {Message}",ex.Message);
                    return HttpStatusCode.InternalServerError;
                }
            }

        }
    }
}
