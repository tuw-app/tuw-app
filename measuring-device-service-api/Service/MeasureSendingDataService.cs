using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace MeasureDeviceServiceAPIProject.Service
{
    public class MeasureSendingDataService : IMeasureSendingDataService, IDisposable
    {

        ILogger<MeasureDevice> logger;

        private double measureingInterval=0;
        private MDDataId dataId;

        private bool lockMesuring = false;
        private bool lockSendingToApi = true;
        private bool stopDevice = false;

        private Timer timer;

        private Queue<char> data = new Queue<char>();


        public void SetMeasureingInterval( double measuringInterval)
        {
            this.measureingInterval = measuringInterval;
        }

        public MeasureSendingDataService(ILogger<MeasureDevice> logger, MDIPAddress IPAddress,double MeasureingInterval) 
        {
            this.logger = logger;
            this.measureingInterval = MeasureingInterval;
            dataId = new MDDataId();
            dataId.IPAddress = IPAddress;

            Initialize();
        }

        public void Start()
        {

        }

        public void Stop()
        {
            if (timer != null)
                timer.Change(Timeout.Infinite,0);
        }

        public void SetMeasuringInterval(int interval)
        {
            this.measureingInterval = interval;
        }

        public double GetMeasuringInterval()
        {
            return this.measureingInterval;
        }



        private void Initialize()
        {
            Log.Information("MeasureDevice {@IpAddress} -> Initialize device...",dataId.IPAddress);
            //https://stackoverflow.com/questions/53727850/how-to-run-backgroundservice-on-a-timer-in-asp-net-core-2-1
            timer = new Timer(MeasuringData, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(measureingInterval));

        }

        private void MeasuringData(object state)
        {
            Log.Information("MeasureDevice {@IpAddress} ->  Measuring data: begin working.");
        }

        public void Dispose()
        {
            if (timer!=null)
                timer.Dispose();
        }
    }
}
