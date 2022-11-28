using MeasureDeviceServiceAPIProject.BackgraoundService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.CodeDom;
using System.Threading;

namespace MeasureDeviceServiceAPIProject.Controllers
{    
    [ApiController]
    public class MDStateContollers : Controller
    {
        ILogger<MDStateContollers> logger = null;
        IMeasureDevice10 device =null;
        
        public MDStateContollers(ILogger<MDStateContollers> logger, IMeasureDevice10 device)
        {
            this.logger= logger;
            this.device= device;
        }

        [HttpGet("api/{IPAddress}/state", Name = "Get state")]
        public IActionResult GetState(string IPAddress)
        {
            if (device == null)
            {
                logger.LogInformation("MDStateContollers -> ControllingMeasuring -> No device.");
                return BadRequest();
            }
            else
            {
                if (IPAddress == null || IPAddress.Length == 0)
                {
                    logger.LogInformation("MDStateContollers -> ControllingMeasuring -> No IP Address.");
                    return BadRequest();
                }

                if (IPAddress.ToString().CompareTo("10.10.10.10") == 0)
                {
                    logger.LogInformation("MDStateContollers -> ControllingMeasuring -> IP Address: 10.10.10.10.");
                    if (device is MeasureDevice10)
                    {
                        logger.LogInformation("MDStateContollers -> ControllingMeasuring -> State: {@State} ", device.MDState);
                        return Ok(device.MDState);
                    }
                    else
                    {
                        logger.LogInformation("MDStateContollers -> ControllingMeasuring -> No device 10.10.10.10.");
                        return BadRequest();
                    }
                }
            }
            logger.LogInformation("MDStateContollers -> ControllingMeasuring -> No State.");
            return BadRequest();
        }
    }
}
