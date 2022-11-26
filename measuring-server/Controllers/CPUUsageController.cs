using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

using MeasureDeviceServiceAPIProject.Model;
using MeasuringServer.Model;
using MeasuringServer.Repository;
using System.Collections.Generic;

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

        [HttpGet("api/cpuusage/{IPAddress}/{Page}/{PageSize}", Name="Get cpu usage of specific device paged")]
        public IActionResult GetAllCPUUsageOfSpecificDevicePaged(string IPAddress,int page, int pageSize)
        {
            //string IPAddress = "10.10.10.10";
            //int page = 1;
            //int pageSize = 50;
            logger.LogInformation("CPUUsageController -> GetAllCPUUsageOfSpecificDevicePaged->Get cpu usage of specific device paged, IPAddress: {Address}, page: {page}", IPAddress, page);

            List<CPUUsageEF> CPUUsages = null;
            try
            {
                if (pageSize == 0)
                    CPUUsages = wrapper.CPUDatas.GetAllCPUUsage();
                else
                    CPUUsages = wrapper.CPUDatas.GetAllCPUUsageOfSpecificDevicePaged(IPAddress, page, pageSize);
                
                if (CPUUsages==null)
                {
                    logger.LogInformation("CPUUsageController -> GetAllCPUUsageOfSpecificDevicePaged->No CPU Usages");
                    return NotFound();
                }

                logger.LogInformation("CPUUsageController -> GetAllCPUUsageOfSpecificDevicePaged->Gets {count} cpu usages");
            }
            catch (Exception exception)
            {
                logger.LogError("CPUUsageController -> GetAllCPUUsageOfSpecificDevicePaged->Error: {Message}",exception.Message);
            }
            return Ok(CPUUsages);
        }


        [HttpPost("api/cpuusage", Name = "Insert new cpu usage")]
        public async Task<IActionResult> InsertNewCpuUsage([FromBody] MDSendedDataFromDeviceToServer data)
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
                logger.LogError("CPUUsageController -> InsertNewCpuUsage->Data is in wrong format. {Message}",exception.Message);
                return BadRequest("Data is in wrong format."); 
            }
            if (dataEF == null || !dataEF.DataIsOk())
            {
                logger.LogInformation("CPUUsageController -> InsertNewCpuUsage->Data is in wrong format. {Data}", dataEF);
                return BadRequest("Data is in wrong format.");
            }

            try
            {
                wrapper.CPUDatas.CreateCPUUsage(dataEF);
                await wrapper.SaveAsync();
            } 
            catch (Exception e)
            {
                if (wrapper.CPUDatas.IsExsist(dataEF.GetId()))
                {
                    logger.LogError("CPUUsageController -> InsertNewCpuUsage-> Data is found in database. Send ok result.");
                    return Ok();
                }
                else
                {
                    logger.LogError("CPUUsageController -> InsertNewCpuUsage-> Failed to insert or IsExsist. {Message}", e.Message);
                    return BadRequest($"Failed to insert {e.Message}");
                }
            }
            logger.LogInformation("CPUUsageController -> InsertNewCpuUsage-> {data} in database!", dataEF);
            return Ok();
        }
    }
}
