using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using MeasureDeviceProject.Service.CPUUsage;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
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

        private Timer timer=null;
        CPUUsageService cuMeasuring=null;

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
            if (timer!=null)
            {
                timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(measureingInterval));
            }
            stopDevice = false;
        }

        public void Stop()
        {
            if (timer != null)
                timer.Change(Timeout.Infinite,0);
            stopDevice= true;
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
            using (LogContext.PushProperty(dataId.IPAddress.ToString(), 1))
            {
                Log.Information("MeasureDevice {@IpAddress} -> Initialize device...", dataId.IPAddress);
                //https://stackoverflow.com/questions/53727850/how-to-run-backgroundservice-on-a-timer-in-asp-net-core-2-1

                cuMeasuring = new CPUUsageService();
                stopDevice = false;
                lockMesuring = false;
                lockSendingToApi = false;

                timer = new Timer(MeasuringData, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(measureingInterval));
            }

        }

        private async void MeasuringData(object state)
        {
            using (LogContext.PushProperty(dataId.IPAddress.ToString(), 1))
            {
                Log.Information("MeasureDevice {IpAddress} -> Measuring data: begin working.", dataId.IPAddress.ToString());
                while (true)
                {

                    if (stopDevice)
                    {
                        Log.Information("MeasureDevice {@IpAddress} ->  Device is stoped.", dataId.IPAddress.ToString());
                    }
                    else
                    {
                        Log.Information("MeasureDevice {@IpAddress} -> prepare to measuring.", dataId.IPAddress.ToString());
                        await cuMeasuring.ReadCPUUsage();
                        DateTime measuringTime = DateTime.Now;
                        Log.Information("MeasureDevice {@IpAddress} -> Measuring time: {Time}", dataId.IPAddress.ToString(), measuringTime.ToString("yyyy.MM.dd HH:mm:ss.ff)"));
                        Log.Information("MeasureDevice {@IpAddress} -> Measuring data: {Data}", dataId.IPAddress.ToString(), cuMeasuring.GetCPUUsageToLog());

                        /*while (lockSendingToApi)
                            Log.Information("MeasuringDevice: Sending locked. Can not save data. Waiting for can save signal.");
                        lockMesuring = true;
                        //data.Enqueue(measuredResult);
                        lockMesuring = false;
                        Log.Information("MeasuringDevice: Measured data have been saved to queue");
                        Thread.Sleep(TimeSpan.FromMilliseconds(measuringInterval));*/

                    }
                }
            }                     
        }

        public void Dispose()
        {
            if (timer!=null)
                timer.Dispose();
            if (cuMeasuring!=null)
                cuMeasuring.Dispose();
                                
        }
    }
}
