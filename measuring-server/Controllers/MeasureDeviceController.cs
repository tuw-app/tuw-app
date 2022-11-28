using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

using MeasuringServer.Model;
using MeasuringServer.Repository;
using System.Collections.Generic;
using MeasuringServer.Model.Paging;
using System.Linq;
using System.Data;
using DataModel.EFDataModel;

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


        [HttpPost("api/md", Name = "Insert or update measure devices")]
        public async Task<IActionResult> InsertOrUpdateMeasureDevice([FromBody] EFMeasureDevice data)
        {

            if (string.IsNullOrEmpty(data.ToString()))
            {
                logger.LogInformation("MeasureDeviceController -> InsertOrUpdateeasureDevice->Null data");
                return BadRequest("Null data.");
            }
            logger.LogInformation("MeasureDeviceController -> InsertOrUpdateeasureDevice->Data {data}", data);

            if (wrapper.MeasureDevices.IsExsist(data))
            {
                logger.LogError("MeasureDeviceController -> InsertOrUpdateeasureDevice-> Data is found in database. Send ok result.");
                return Ok();
            }
            else
            {
                try
                {
                    if (wrapper.MeasureDevices.IsExsist(data.Name))
                    {
                        // ha van akkor meghatározzuk az id-jét és frissítjük az intervallumot
                        EFMeasureDevice device = wrapper.MeasureDevices.GetByIPAddress(data.Name);
                        data.Id = device.Id;
                        wrapper.MeasureDevices.Update(device.Id,data.Interval);
                        logger.LogInformation("MeasureDeviceController -> InsertOrUpdateeasureDevice-> {data} is updated in database!", data);
                    }
                    {
                        // ha még nem létezik, akkor létrehozzul
                        wrapper.MeasureDevices.Insert(data);
                        logger.LogInformation("MeasureDeviceController -> InsertOrUpdateeasureDevice-> {data} is inserted in database!", data);
                    }
                    await wrapper.SaveAsync();
                }
                catch (Exception e)

                {
                    logger.LogError("MeasureDeviceController -> InsertOrUpdateeasureDevice-> Failed to insert or IsExsist. {Message}", e.Message);
                    return BadRequest($"Failed to insert {e.Message}");
                }                
                return Ok();
            }
        }
    }
}
