using System;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MeasureDeviceProject.BackgraoundService;
using Microsoft.Extensions.Logging;
using MeasureDeviceServiceAPIProject.Model;

namespace MeasureDeviceServiceAPIProject.APIService
{
    public class CPUAPIService
    {

        ILogger<MeasureDevice> logger = null;

        public CPUAPIService(ILogger<MeasureDevice> logger)
        {
            this.logger= logger;
        }

        public async Task<HttpStatusCode> SendNewCPUDataAsync(string dataFromBackupFile)
        {
            //Uri u = new Uri("http://localhost:5001/api/cpuusage");
            UriBuilder u = new UriBuilder();
            u.Scheme= "http";
            u.Host = "localhost";
            u.Path = "CPUUsage/api/cpuusage";
            u.Port = 5001;

            logger.LogInformation("CPUAPIService -> Uri is {Uri}", u.ToString());

            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = u.Uri;

                    MDSendedDataFromDeviceToServer sendedData = new MDSendedDataFromDeviceToServer(dataFromBackupFile);
                    logger.LogInformation("CPUAPIService -> Sended data -> {data}", sendedData);

                    String jsonString = JsonConvert.SerializeObject(sendedData);
                    StringContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    //logger.LogInformation("CPUAPIService -> StringContent-> {Content}", httpContent.Headers);

                    var response = await httpClient.PostAsync(u.Uri, httpContent);

                    if (response != null)
                    {
                        logger.LogInformation("CPUAPIService -> StringContent-> Status code: {Code}", response.StatusCode);
                        if (response.StatusCode == HttpStatusCode.OK)
                            return HttpStatusCode.OK;
                        else
                        {

                            string error = response.Headers + " : " + response.Content + " : " + response.StatusCode;
                            logger.LogInformation("CPUAPIService -> StringContent-> Error {error}", error.ToString());
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
