using DataModel.MDDataModel;
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
        IMeasureDevice10 device10 = null;
        IMeasureDevice20 device20 = null;
        IMeasureDevice30 device30 = null;

        public MDIntervalContoller(ILogger<MDIntervalContoller> logger, IMeasureDevice10 device10, IMeasureDevice20 device20, IMeasureDevice30 device30)
        {
            this.logger= logger;
            this.device10 = device10;
            this.device20 = device20;
            this.device30 = device30;
        }

        [HttpGet("api/interval/{IPAddress}", Name = "Get measure device interval")]
        public IActionResult GetMDInterval(string IPAddress)
        {
            if  ((device10 == null) || (device20 == null) || (device30 == null))
                {
                logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> No device.");
                return BadRequest();
            }
            else
            {
                if (IPAddress == null || IPAddress.Length == 0)
                {
                    logger.LogInformation("{MDIntervalContoller -> ControllingMeasuring -> No IP Address.");
                    return BadRequest();
                }

                if (IPAddress.ToString().CompareTo("10.10.10.10") == 0)
                {
                    logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> IP Address: 10.10.10.10.");
                    if (device10 is MeasureDevice10)
                    {
                        logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> State: {@State} ", device10.GetInterval());
                        return Ok(device10.GetInterval());
                    }
                    else
                    {
                        logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> No device 10.10.10.10.");
                        return BadRequest();
                    }
                }

                if (IPAddress.ToString().CompareTo("20.20.20.20") == 0)
                {
                    logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> IP Address: 20.20.20.20");
                    if (device20 is MeasureDevice20)
                    {
                        logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> State: {@State} ", device20.GetInterval());
                        return Ok(device20.GetInterval());
                    }
                    else
                    {
                        logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> No device 20.20.20.20");
                        return BadRequest();
                    }
                }

                if (IPAddress.ToString().CompareTo("30.30.30.30") == 0)
                {
                    logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> IP Address: 10.10.10.10.");
                    if (device30 is MeasureDevice30)
                    {
                        logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> State: {@State} ", device30.GetInterval());
                        return Ok(device30.GetInterval());
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
        public IActionResult SetMDInsterval(string IPAddress, [FromBody] MDIntervalData interval)
        {
            if ((device10 == null) || (device20 == null) || (device30 == null))
            {
                logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> No device.");
                return BadRequest();
            }
            else
            {
                if (IPAddress == null || IPAddress.Length == 0)
                {
                    logger.LogInformation("{MDIntervalContoller -> ControllingMeasuring -> No IP Address.");
                    return BadRequest();
                }

                if (IPAddress.ToString().CompareTo("10.10.10.10") == 0)
                {
                    logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> IP Address: 10.10.10.10.");
                    if (device10 is MeasureDevice10)
                    {
                        logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> Interval: {Interval} ", interval);
                        device10.SetInterval(interval.Interval);
                        return Ok();
                    }
                    else
                    {
                        logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> No device 10.10.10.10.");
                        return BadRequest();
                    }
                }

                if (IPAddress.ToString().CompareTo("20.20.20.20") == 0)
                {
                    logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> IP Address: 20.20.20.20.");
                    if (device20 is MeasureDevice20)
                    {
                        logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> Interval: {Interval} ", interval);
                        device20.SetInterval(interval.Interval);
                        return Ok();
                    }
                    else
                    {
                        logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> No device 20.20.20.20.");
                        return BadRequest();
                    }
                }

                if (IPAddress.ToString().CompareTo("30.30.30.30") == 0)
                {
                    logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> IP Address: 30.30.30.30");
                    if (device30 is MeasureDevice30)
                    {
                        logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> Interval: {Interval} ", interval);
                        device30.SetInterval(interval.Interval);
                        return Ok();
                    }
                    else
                    {
                        logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> No device 30.30.30.30");
                        return BadRequest();
                    }
                }

            }
            logger.LogInformation("MDIntervalContoller -> ControllingMeasuring -> No State.");
            return BadRequest();
        }

    }
}
