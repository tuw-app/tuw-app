using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

using MeasureDeviceServiceAPIProject.Model;
using MeasuringServer.Model;
using MeasuringServer.Repository;
using System.Collections.Generic;
using MeasuringServer.Model.Paging;
using System.Linq;
using System.Data;

namespace MeasuringServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MeasureDeviceController : Controller
    {
        private IRepositoryWrapper wrapper=null;
        ILogger<MeasureDeviceController> logger=null;

        public MeasureDeviceController(ILogger<MeasureDeviceController> logger, IRepositoryWrapper wrapper)
        {
            logger.LogInformation("MeasureDeviceController");
            this.logger = logger;
            this.wrapper = wrapper;
        }

        [HttpGet("api/md", Name="Get all measure devices")]
        public IActionResult GetAllMeasureDevices()
        {
            logger.LogInformation("MeasureDeviceController -> GetAllMeasureDevices");

            List<EFMeasureDevice> measureDevices = null;
            try
            {
                measureDevices = wrapper.MeasureDevices.GetAll().ToList();
                if (measureDevices == null)
                {
                    logger.LogInformation("MeasureDeviceController -> GetAllMeasureDevices->No measured devices");
                    return NotFound();
                }              
            }
            catch (Exception exception)
            {
                logger.LogError("MeasureDeviceController -> GetAllCPUUsageOfSpecificDevicePaged->Error: {Message}", exception.Message);
            }
            logger.LogInformation("MeasureDeviceController -> GetAllMeasureDevices->Gets  {number} measure devices", measureDevices.Count);
            return Ok(measureDevices);

        }

        [HttpGet("api/md/{IPAddress}", Name = "Get measure devices by IPAddress")]
        public IActionResult GetMeasureDeviceByAddress(string IPAddress)
        {
            logger.LogInformation("MeasureDeviceController -> GetMeasureDeviceByAddress");

            if (IPAddress == null || IPAddress.Length == 0)
            {
                logger.LogInformation("{MeasureDeviceController -> GetMeasureDeviceByAddress -> No IP Address.");
                return BadRequest();
            }

            EFMeasureDevice measureDevice = null;
            try
            {
                measureDevice = wrapper.MeasureDevices.GetByIPAddress(IPAddress);
                if (measureDevice == null)
                {
                    logger.LogInformation("MeasureDeviceController -> GetMeasureDeviceByAddress->No measured devices");
                    return NotFound();
                }
            }
            catch (Exception exception)
            {
                logger.LogError("MeasureDeviceController -> GetAllCPUUsageOfSpecificDevicePaged->Error: {Message}", exception.Message);
            }
            logger.LogInformation("MeasureDeviceController -> GetMeasureDeviceByAddress->Gets  {@Device} measure devices", measureDevice);
            return Ok(measureDevice);

        }


        [HttpPost("api/md", Name = "Insert new measure devices")]
        public async Task<IActionResult> InsertNewMeasureDevice([FromBody] EFMeasureDevice data)
        {

            if (string.IsNullOrEmpty(data.ToString()))
            {
                logger.LogInformation("MeasureDeviceController -> InsertNewMeasureDevice->Null data");
                return BadRequest("Null data.");
            }
            logger.LogInformation("MeasureDeviceController -> InsertNewMeasureDevice->Data {data}", data);

            if (wrapper.MeasureDevices.IsExsist(data))
            {
                logger.LogError("MeasureDeviceController -> InsertNewMeasureDevice-> Data is found in database. Send ok result.");
                return Ok();
            }
            else
            {
                try
                {
                    wrapper.MeasureDevices.Insert(data);
                    await wrapper.SaveAsync();
                }
                catch (Exception e)

                {
                    logger.LogError("MeasureDeviceController -> InsertNewMeasureDevice-> Failed to insert or IsExsist. {Message}", e.Message);
                    return BadRequest($"Failed to insert {e.Message}");
                }
                logger.LogInformation("MeasureDeviceController -> InsertNewMeasureDevice-> {data} in database!", data);
                return Ok();
            }
        }
    }
}
