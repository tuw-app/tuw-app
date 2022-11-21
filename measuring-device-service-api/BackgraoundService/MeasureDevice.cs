using MeasureDeviceProject.Model.DataSendingElements;
using MeasureDeviceProject.Model.SubDevices;
using MeasureDeviceServiceAPIProject.BackgraoundService;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MeasureDeviceProject.Model
{
    public class MeasureDevice : BackgroundService, IMeasureDevice
    {
        private MDIPAddress MDIPAddress { get; set; }
        private MDDataId dataId;
        private DataSendingSubDevice dataSendingSubDevice;
        private MesuringSubDevice MesuringSubDevice;

        public MeasureDevice()
        {
        }

        public void StartDevice()
        {
            throw new NotImplementedException();
        }

        public void StopDevice()
        {
            throw new NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
