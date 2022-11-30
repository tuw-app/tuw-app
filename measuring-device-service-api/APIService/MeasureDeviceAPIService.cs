using System;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MeasureDeviceProject.BackgraoundService;
using Microsoft.Extensions.Logging;
using DataModel.MDDataModel;
using DataModel.EFDataModel;

namespace MeasureDeviceServiceAPIProject.APIService
{
    public class MeasureDeviceAPIService
    {

        ILogger<MeasureDevice> logger = null;

        public MeasureDeviceAPIService(ILogger<MeasureDevice> logger)
        {
            this.logger= logger;
        }

        public async Task<HttpStatusCode> SendMDDataToAsync(EFMeasureDevice measureDevice)
        {
            //Uri u = new Uri("http://localhost:5001/MeasureDeviceController/api/md/{IPAddress}");
            UriBuilder u = new UriBuilder();
            u.Scheme= "http";
            u.Host = "localhost";
            u.Path = "/api/md/";
            u.Port = 5001;

            logger.LogInformation("MeasureDeviceAPIService -> Uri is {Uri}", u.ToString());

            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = u.Uri;

                    logger.LogInformation("MeasureDeviceAPIService -> Sended data -> {@data}", measureDevice);

                    String jsonString = JsonConvert.SerializeObject(measureDevice);
                    StringContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    //logger.LogInformation("CPUAPIService -> StringContent-> {Content}", httpContent.Headers);

                    var response = await httpClient.PostAsync(u.Uri, httpContent);

                    if (response != null)
                    {
                        logger.LogInformation("MeasureDeviceAPIService -> StringContent-> Status code: {Code}", response.StatusCode);
                        if (response.StatusCode == HttpStatusCode.OK)
                            return HttpStatusCode.OK;
                        else
                        {

                            string error = response.Headers + " : " + response.Content + " : " + response.StatusCode;
                            logger.LogInformation("MeasureDeviceAPIService -> StringContent-> Error {error}", error.ToString());
                            return HttpStatusCode.InternalServerError;
                        }
                    }
                    else
                    {
                        logger.LogInformation("MeasureDeviceAPIService -> StringContent-> Response is null");
                        return HttpStatusCode.InternalServerError;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogInformation("CPUAPIService -> MeasureDeviceAPIService-> Exception {Message}", ex.Message);
                    return HttpStatusCode.InternalServerError;
                }
            }

        }
    }
}
