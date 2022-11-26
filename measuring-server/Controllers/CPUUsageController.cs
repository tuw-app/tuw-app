using AutoMapper;
using MeasureDeviceProject.Model;
using MeasureDeviceProject.Model.CPUUsageModel;
using MeasuringServer.Model;
using MeasuringServer.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MeasuringServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CPUUsageController : Controller
    {
        private IRepositoryWrapper wrapper=null;
        ILogger<CPUUsageController> logger=null;


        public CPUUsageController(ILogger<CPUUsageController> logger, IRepositoryWrapper wrapper)
        {
            logger.LogInformation("CPUUsageController");
            this.logger = logger;
            this.wrapper = wrapper;
        }

        [HttpPost("api/cpuusage", Name = "Insert new cpu usage")]
        public async Task<IActionResult> InsertNewCpuUsage([FromBody] MDSended data)
        {

            if (string.IsNullOrEmpty(data.ToString()))
            {
                logger.LogInformation("CPUUsageController -> InsertNewCpuUsage->Null data");
                return BadRequest("Null data.");
            }
            logger.LogInformation("CPUUsageController -> InsertNewCpuUsage->Data {data}",data);
            CPUUsageEF dataEF = null;
            try
            {
                dataEF = new CPUUsageEF(data);
                logger.LogInformation("CPUUsageController -> InsertNewCpuUsage->Text->Usage {data}", dataEF);
            }
            catch (Exception exception)
            {
                logger.LogInformation("CPUUsageController -> InsertNewCpuUsage->Data is in wrong format. {Message}",exception.Message);
                return BadRequest("Data is in wrong format."); 
            }
            if (dataEF == null || !dataEF.DataIsOk())
            {
                logger.LogInformation("CPUUsageController -> InsertNewCpuUsage->Data is in wrong format. {Data}", dataEF);
                return BadRequest("Data is in wrong format.");
            }

            try
            {
                logger.LogInformation("CPUUsageController -> InsertNewCpuUsage-> Insert {Data}", dataEF);
                wrapper.CPUDatas.CreateCPUUsage(dataEF);
                logger.LogInformation("CPUUsageController -> InsertNewCpuUsage-> Before save");
                await wrapper.SaveAsync();
            } 
            catch (Exception e)
            {
                logger.LogInformation("CPUUsageController -> InsertNewCpuUsage-> Failed to insert. {Message}", e.Message);
                return BadRequest($"Failed to insert {e.Message}");
            }
            logger.LogInformation("CPUUsageController -> InsertNewCpuUsage-> {data} in database!", dataEF);
            return Ok();
        }
    }
}
