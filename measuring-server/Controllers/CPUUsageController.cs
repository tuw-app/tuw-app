using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

using MeasuringServer.Repository;
using System.Collections.Generic;
using MeasuringServer.Model.Paging;
using System.Linq;
using DataModel.EFDataModel;
using DataModel.MDDataModel;

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
            logger.LogInformation("CPUUsageController -> GetAllCPUUsageOfSpecificDevicePaged->Get cpu usage of specific device paged, IPAddress: {Address}, page: {page}", IPAddress, page);

            List<EFCPUUsage> CPUUsages = null;
            PagedList<EFCPUUsage> CPUUsagesPaged = null;
            try
            {                
                if (pageSize == 0 || page==0)
                {
                    // No paged
                    CPUUsages = wrapper.CPUDatas.GetAllCPUUsage();
                    if (CPUUsages == null)
                    {
                        logger.LogInformation("CPUUsageController -> GetAllCPUUsageOfSpecificDevicePaged->No CPU Usages");
                        return NotFound();
                    }
                    else
                    {
                        if (IPAddress.Length==0)
                        {
                            // Get all
                            logger.LogInformation("CPUUsageController -> GetAllCPUUsageOfSpecificDevicePaged->Gets all {count} cpu usages", CPUUsages.Count);
                            return Ok(CPUUsages);
                        }
                        else
                        {
                            // No paged, select by IP address
                            logger.LogInformation("CPUUsageController -> GetAllCPUUsageOfSpecificDevicePaged->Gets cpu usages from {IPAddress} IP address", IPAddress);
                            return Ok(CPUUsages.Where(cpuUsage => cpuUsage.IPAddress.CompareTo(IPAddress)==0).ToList());
                        }
                    }
                }
                else
                {
                    // Get all paged
                    CPUUsagesPaged = wrapper.CPUDatas.GetAllCPUUsageOfSpecificDevicePaged(IPAddress, page, pageSize);
                    logger.LogInformation("CPUUsageController -> GetAllCPUUsageOfSpecificDevicePaged->Gets paged {PageInfo} cpu usages",CPUUsagesPaged.ToString());
                }

                logger.LogInformation("CPUUsageController -> GetAllCPUUsageOfSpecificDevicePaged->Gets {count} cpu usages");
            }
            catch (Exception exception)
            {
                logger.LogError("CPUUsageController -> GetAllCPUUsageOfSpecificDevicePaged->Error: {Message}",exception.Message);
            }
            return Ok(CPUUsagesPaged);

        }


        [HttpPost("api/cpuusage", Name = "Insert new cpu usage")]
        public async Task<IActionResult> InsertNewCPUUsage([FromBody] MDSendedDataFromDeviceToServer data)
        {

            if (string.IsNullOrEmpty(data.ToString()))
            {
                logger.LogInformation("CPUUsageController -> InsertNewCpuUsage->Null data");
                return BadRequest("Null data.");
            }
            logger.LogInformation("CPUUsageController -> InsertNewCpuUsage->Data {data}",data);
            EFCPUUsage dataEF = null;
            try
            {
                dataEF = new EFCPUUsage(data);
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
