using MeasureDeviceServiceAPIProject.BackgraoundService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.CodeDom;
using System.Threading;

namespace MeasureDeviceServiceAPIProject.Controllers
{    
    [ApiController]
    public class ControllingContollers : Controller
    {
        ILogger<ControllingContollers> logger = null;
        IMeasureDevice10 device =null;
        
        public ControllingContollers(ILogger<ControllingContollers> logger, IMeasureDevice10 device)
        {
            this.logger= logger;
            this.device= device;
        }

        [HttpGet("api/{controlling}/{IPAddress}", Name="Stop measure device working")]
        public IActionResult ControllingMeasuring(string controlling, string IPAddress)
        {

            if (device == null)
            {
                logger.LogInformation("ControllingContollers -> ControllingMeasuring -> No device.");
            }
            else 
            {
                if (IPAddress.ToString().CompareTo("10.10.10.10") == 0)
                {
                    if (device is MeasureDevice10)
                    {
                        MeasureDevice10 device10 = (MeasureDevice10)device;
                        if (device10 != null)
                        {
                            if (controlling == "stop")
                            {
                                logger.LogInformation("ControllingContollers -> ControllingMeasuring -> Controlling: {Controlling}, IP address: {Address}.", controlling, IPAddress);

                                CancellationToken token = new CancellationToken(true);
                                if (token.IsCancellationRequested)
                                {
                                    logger.LogInformation("Controllr->Token stop cancel is requested");
                                }
                                else
                                {
                                    logger.LogInformation("Controllr->Token stop cancel is not requested");
                                }
                                device10.StopAsync(token);

                            }
                            else if (controlling == "start")
                            {
                                CancellationToken token = new CancellationToken(false);

                                if (token.IsCancellationRequested)
                                {
                                    logger.LogInformation("Controllr->Token starrt cancel is requested");
                                }
                                else
                                {
                                    logger.LogInformation("Controllr->Token stop cancel is not requested");
                                }
                                device10.StartAsync(token);
                            }
                            else if (controlling == "measuring")
                            {
                                device10.StartMeasuring();
                            }
                            else if (controlling == "nomeasuring")
                            {
                                device10.StopMeasuring();
                            }
                        }
                    }
                }
            }
            return Ok();
        }

    }
}
