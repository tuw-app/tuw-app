using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using Microsoft.Extensions.Logging;

namespace MeasureDeviceServiceAPIProject.Service
{
    public class MeasureSendingDataService
    {

        ILogger<MeasureSendingDataService> logger;

        private double measureingInterval=0;
        private MDDataId dataId;

        public void SetMeasureingInterval(double measuringInterval)
        {
            this.measureingInterval = measuringInterval;
        }

        public MeasureSendingDataService(double MeasureingInterval) 
        {
            //this.logger = logger;
            this.measureingInterval = MeasureingInterval;
        }


    }
}
