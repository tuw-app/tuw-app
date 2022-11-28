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
        IMeasureDevice10 device10 =null;
        IMeasureDevice20 device20 = null;
        IMeasureDevice30 device30 = null;

        public MDStateContollers(ILogger<MDStateContollers> logger, IMeasureDevice10 device10, IMeasureDevice20 device20, IMeasureDevice30 device30)
        {
            this.logger= logger;
            this.device10= device10;
            this.device20 = device20;
            this.device30= device30;
        }

        [HttpGet("api/{IPAddress}/state", Name = "Get state")]
        public IActionResult GetState(string IPAddress)
        {
            if ((device10 == null) || (device20 == null) || (device30 == null))
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

                // 10. 10. 10. 10

                if (IPAddress.ToString().CompareTo("10.10.10.10") == 0)
                {
                    logger.LogInformation("MDStateContollers -> ControllingMeasuring -> IP Address: 10.10.10.10.");
                    if (device10 is MeasureDevice10)
                    {
                        logger.LogInformation("MDStateContollers -> ControllingMeasuring -> State: {@State} ", device10.MDState);
                        return Ok(device10.MDState);
                    }
                    else
                    {
                        logger.LogInformation("MDStateContollers -> ControllingMeasuring -> No device 10.10.10.10.");
                        return BadRequest();
                    }
                }


                // 20. 20. 20. 20

                if (IPAddress.ToString().CompareTo("20.20.20.20") == 0)
                {
                    logger.LogInformation("MDStateContollers -> ControllingMeasuring -> IP Address: 20.20.20.20.");
                    if (device20 is MeasureDevice20)
                    {
                        logger.LogInformation("MDStateContollers -> ControllingMeasuring -> State: {@State} ", device20.MDState);
                        return Ok(device20.MDState);
                    }
                    else
                    {
                        logger.LogInformation("MDStateContollers -> ControllingMeasuring -> No device 20.20.20.20.");
                        return BadRequest();
                    }
                }

                // 30. 30. 30. 30

                if (IPAddress.ToString().CompareTo("30.30.30.30") == 0)
                {
                    logger.LogInformation("MDStateContollers -> ControllingMeasuring -> IP Address: 30.30.30.30.");
                    if (device30 is MeasureDevice30)
                    {
                        logger.LogInformation("MDStateContollers -> ControllingMeasuring -> State: {@State} ", device30.MDState);
                        return Ok(device30.MDState);
                    }
                    else
                    {
                        logger.LogInformation("MDStateContollers -> ControllingMeasuring -> No device 30.30.30.30.");
                        return BadRequest();
                    }
                }

            }
            logger.LogInformation("MDStateContollers -> ControllingMeasuring -> No State.");
            return BadRequest();
        }
    }
}
