using MeasureDeviceProject.Model;
using MeasureDeviceProject.Model.DataSendingElements;
using MeasureDeviceProject.Model.SubDevices;
using MeasureDeviceServiceAPIProject.BackgraoundService;
using Microsoft.Extensions.Logging;

namespace MeasureDeviceServiceAPIProject.Service
{
    public class MeasurSendingDataService
    {
        private double measureingInterval=0;
        private MDDataId dataId;

        public void SetMeasureingInterval(double measuringInterval)
        {
            this.measureingInterval = measuringInterval;
        }

        public MeasurSendingDataService(ILogger<MEFactory> logger, double MeasureingInterval) 
        {
            this.measureingInterval = MeasureingInterval;
        }


    }
}
