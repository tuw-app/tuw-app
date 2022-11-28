using MeasureDeviceServiceAPIProject.BackgraoundService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.CodeDom;
using System.Threading;

namespace MeasureDeviceServiceAPIProject.Controllers
{    
    [ApiController]
    public class MDIntervalContoller : Controller
    {
        ILogger<MDIntervalContoller> logger = null;
        IMeasureDevice10 device =null;
        
        public MDIntervalContoller(ILogger<MDIntervalContoller> logger, IMeasureDevice10 device)
        {
            this.logger= logger;
            this.device= device;
        }

        [HttpGet("api/interval/{IPAddress}", Name = "Get measure device interval")]
        public IActionResult GetMDInterval(string IPAddress)
        {
            if (device == null)
            {
                logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> No device.");
                return BadRequest();
            }
            else
            {
                if (IPAddress != null || IPAddress.Length != 0)
                {
                    logger.LogInformation("{MDIntervalContoller -> ControllingMeasuring -> No IP Address.");
                    return BadRequest();
                }

                if (IPAddress.ToString().CompareTo("10.10.10.10") == 0)
                {
                    logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> IP Address: 10.10.10.10.");
                    if (device is MeasureDevice10)
                    {
                        logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> State: {@State} ", device.MDState);
                        return Ok(device.MeasuringInterval);
                    }
                    else
                    {
                        logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> No device 10.10.10.10.");
                        return BadRequest();
                    }
                }
            }
            logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> No State.");
            return BadRequest();
        }

        [HttpPost("api/interval/{IPAddress}", Name = "Set measure device interval")]
        public IActionResult SetMDInsterval(string IPAddress, [FromBody] int interval)
        {
            if (device == null)
            {
                logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> No device.");
                return BadRequest();
            }
            else
            {
                if (IPAddress != null || IPAddress.Length != 0)
                {
                    logger.LogInformation("{MDIntervalContoller -> ControllingMeasuring -> No IP Address.");
                    return BadRequest();
                }

                if (IPAddress.ToString().CompareTo("10.10.10.10") == 0)
                {
                    logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> IP Address: 10.10.10.10.");
                    if (device is MeasureDevice10)
                    {
                        logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> Interval: {Interval} ", interval);
                        device.MeasuringInterval = interval;
                        return Ok();
                    }
                    else
                    {
                        logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> No device 10.10.10.10.");
                        return BadRequest();
                    }
                }
            }
            logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> No State.");
            return BadRequest();
        }

    }
}
