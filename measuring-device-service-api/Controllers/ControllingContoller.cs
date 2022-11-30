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
        IMeasureDevice10 device10 = null;
        IMeasureDevice20 device20 = null;
        IMeasureDevice30 device30 = null;

        public ControllingContoller(ILogger<ControllingContoller> logger, IMeasureDevice10 device10, IMeasureDevice20 device20, IMeasureDevice30 device30)
        {
            this.logger= logger;
            this.device10 = device10;
            this.device20 = device20;
            this.device30 = device30;
        }

        [HttpGet("api/{controlling}/{IPAddress}", Name="Controll of measure device work")]
        public async Task<IActionResult> ControllingMeasuring(string controlling, string IPAddress)
        {

            if (device10 == null)
            {
                logger.LogInformation("ControllingContollers -> ControllingMeasuring -> No device.");
                return BadRequest();
            }
            else 
            {
                if (IPAddress == null || IPAddress.Length == 0)
                {
                    logger.LogInformation("ControllingContollers -> ControllingMeasuring -> No IP address.");
                    return BadRequest();
                }


                if (IPAddress.ToString().CompareTo("10.10.10.10") == 0)
                {
                    logger.LogInformation("ControllingContollers -> ControllingMeasuring -> IP address: 10.10.10.10");
                    if (device10 is MeasureDevice10)
                    {
                        if (device10 != null)
                        {
                            if (controlling == "stop")
                            {
                                logger.LogInformation("ControllingContollers -> ControllingMeasuring -> Controlling: {Controlling}, IP address: {Address}.", controlling, IPAddress);

                                device10.StopAsync();

                                logger.LogInformation("ControllingContollers->Stop Async");
                                return Ok();

                            }
                            else if (controlling == "start")
                            {
  
                                device10.StartAsync();

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



                if (IPAddress.ToString().CompareTo("20.20.20.20") == 0)
                {
                    logger.LogInformation("ControllingContollers -> ControllingMeasuring -> IP address: 10.10.10.10");
                    if (device20 is MeasureDevice20)
                    {
                        if (device20 != null)
                        {
                            if (controlling == "stop")
                            {
                                logger.LogInformation("ControllingContollers -> ControllingMeasuring -> Controlling: {Controlling}, IP address: {Address}.", controlling, IPAddress);

                                device20.StopAsync();

                                logger.LogInformation("ControllingContollers->Stop Async");
                                return Ok();

                            }
                            else if (controlling == "start")
                            {

                                device20.StartAsync();

                                logger.LogInformation("ControllingContollers->Start Async");
                                return Ok();
                            }
                            else if (controlling == "measuring")
                            {
                                device20.StartMeasuring();
                                logger.LogInformation("ControllingContollers->Start Measuring");
                                return Ok();
                            }
                            else if (controlling == "nomeasuring")
                            {
                                device20.StopMeasuring();
                                logger.LogInformation("ControllingContollers->Stop Measuring");
                                return Ok();
                            }
                        }
                    }
                }

                if (IPAddress.ToString().CompareTo("30.30.30.30") == 0)
                {
                    logger.LogInformation("ControllingContollers -> ControllingMeasuring -> IP address: 30.30.30.30");
                    if (device30 is MeasureDevice30)
                    {
                        if (device30 != null)
                        {
                            if (controlling == "stop")
                            {
                                logger.LogInformation("ControllingContollers -> ControllingMeasuring -> Controlling: {Controlling}, IP address: {Address}.", controlling, IPAddress);

                                device30.StopAsync();

                                logger.LogInformation("ControllingContollers->Stop Async");
                                return Ok();

                            }
                            else if (controlling == "start")
                            {

                                device30.StartAsync();

                                logger.LogInformation("ControllingContollers->Start Async");
                                return Ok();
                            }
                            else if (controlling == "measuring")
                            {
                                device30.StartMeasuring();
                                logger.LogInformation("ControllingContollers->Start Measuring");
                                return Ok();
                            }
                            else if (controlling == "nomeasuring")
                            {
                                device30.StopMeasuring();
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
