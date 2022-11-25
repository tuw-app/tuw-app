using System;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MeasureDeviceServiceAPIProject.APIService
{
    public class CPUAPIService
    {
        public async Task<HttpStatusCode> InsertNewCPUDataAsync(string CPUdata)
        {
            Uri u = new Uri("http://localhost:5001/api/cpuusage");

            using (var httpClient = new HttpClient())
            {                
                try
                {
                    String jsonString = JsonConvert.SerializeObject(CPUdata);
                    StringContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("/Subject/api/subject", httpContent);

                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.Created)
                            return HttpStatusCode.OK;
                        else
                        {

                            string error = response.Headers + " : " + response.Content + " : " + response.StatusCode;
                            Console.WriteLine(error);
                            return HttpStatusCode.InternalServerError;
                        }
                    }
                    else
                        return HttpStatusCode.InternalServerError;
                }
                catch (Exception ex)
                {
                    return HttpStatusCode.InternalServerError;
                }
            }

        }
    }
}
