using MeasureDeviceServiceAPIProject.BackgraoundService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MeasureDeviceServiceAPIProject.Controllers
{    
    [ApiController]
    public class ControllingContoller : Controller
    {
        ILogger<ControllingContoller> logger = null;
        IMeasureDevice10 device =null;
        
        public ControllingContoller(ILogger<ControllingContoller> logger, IMeasureDevice10 device)
        {
            this.logger= logger;
            this.device= device;
        }

        [HttpGet("api/{controlling}/{IPAddress}", Name="Controll of measure device work")]
        public async Task<IActionResult> ControllingMeasuring(string controlling, string IPAddress)
        {

            if (device == null)
            {
                logger.LogInformation("ControllingContollers -> ControllingMeasuring -> No device.");
                return BadRequest();
            }
            else 
            {
                if (IPAddress != null || IPAddress.Length != 0)
                {
                    logger.LogInformation("ControllingContollers -> ControllingMeasuring -> No IP address.");
                    return BadRequest();
                }

                if (IPAddress.ToString().CompareTo("10.10.10.10") == 0)
                {
                    logger.LogInformation("ControllingContollers -> ControllingMeasuring -> IP address: 10.10.10.10");
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
                                    logger.LogInformation("ControllingContollers->Token stop cancel is requested");
                                }
                                else
                                {
                                    logger.LogInformation("ControllingContollers->Token stop cancel is not requested");
                                }
                                await device10.StopAsync(token);
                                logger.LogInformation("ControllingContollers->Stop Async");
                                return Ok();

                            }
                            else if (controlling == "start")
                            {
                                CancellationToken token = new CancellationToken(false);

                                if (token.IsCancellationRequested)
                                {
                                    logger.LogInformation("ControllingContollers->Token starrt cancel is requested");
                                }
                                else
                                {
                                    logger.LogInformation("ControllingContollers->Token stop cancel is not requested");
                                }                                
                                await device10.StartAsync(token);
                                logger.LogInformation("ControllingContollers->Start Async");
                                return Ok();
                            }
                            else if (controlling == "measuring")
                            {
                                device10.StartMeasuring();
                                logger.LogInformation("ControllingContollers->Start Measuring");
                                return Ok();
                            }
                            else if (controlling == "nomeasuring")
                            {
                                device10.StopMeasuring();
                                logger.LogInformation("ControllingContollers->Stop Measuring");
                                return Ok();
                            }
                        }
                    }                    
                }
            }
            logger.LogInformation("ControllingContollers->No device");
            return BadRequest();
        }

    }
}
