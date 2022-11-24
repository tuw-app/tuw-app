using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using MeasureDeviceProject.Service.CPUUsage;
using MeasureDeviceProject.Service.FileWriter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace MeasureDeviceServiceAPIProject.Service
{
    public class MeasureSendingDataService : IMeasureSendingDataService, IDisposable
    {

        ILogger<MeasureDevice> logger;
        IConfiguration configuration;

        private double measureingInterval=0;
        private MDDataId dataId;

        private bool lockMesuring = false;
        private bool lockSendingToApi = true;
        private bool stopDevice = false;

        private Timer timer=null;
        CPUUsageService cuMeasuring=null;
        MeasuringDataStore cpuDataStore = null;

        private string path = string.Empty;

        private Queue<string> dataQueue = new Queue<string>();

        public void SetMeasureingInterval( double measuringInterval)
        {
            this.measureingInterval = measuringInterval;
        }

        public MeasureSendingDataService(IConfiguration configuration, ILogger<MeasureDevice> logger, MDIPAddress IPAddress,double MeasureingInterval) 
        {
            this.logger = logger;
            this.configuration = configuration;
            this.measureingInterval = MeasureingInterval;
            dataId = new MDDataId(IPAddress);
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

                path = configuration["Path"]; // ???????
                Log.Information("MeasureDevice {@IpAddress} -> The log path is {Path}", dataId.IPAddress,path);
                
                cpuDataStore = new MeasuringDataStore(logger,"CPU", "d:\\tuw\\log\\"+ dataId.IPAddress+"\\", "store", dataId);
                Log.Information("MeasureDevice {@IpAddress} -> The log file name is {file}", dataId.IPAddress, cpuDataStore.ToString());

                // Miért nem működik 
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
                        // Mesuring
                        await cuMeasuring.ReadCPUUsage();
                        DateTime measuringTime = DateTime.Now;
                        Log.Information("MeasureDevice {@IpAddress} -> Measuring time: {Time}", dataId.IPAddress.ToString(), measuringTime.ToString("yyyy.MM.dd HH:mm:ss.ff)"));
                        Log.Information("MeasureDevice {@IpAddress} -> Measuring data: {Data}", dataId.IPAddress.ToString(), cuMeasuring.GetCPUUsageToLog());
                        // Store Data to log file
                        StringBuilder sb = new StringBuilder();
                        lock (dataId)
                        {
                            sb.Append(dataId.IPAddress).Append(dataId.MeasuringId).Append(";").Append(dataId.DataID).Append(";").Append(cuMeasuring.GetCPUUsage());
                            try
                            {
                                cpuDataStore.WriteData(sb.ToString());
                                Log.Information("MeasureDevice {@IpAddress} -> Measuring data stored in logfile.", dataId.IPAddress.ToString());
                            }
                            catch (Exception ex)
                            {
                                Log.Error("MeasureDevice {@IpAddress} -> Measuring write data: {Data}", dataId.IPAddress.ToString(), cuMeasuring.GetCPUUsageToLog());
                            }
                            

                            Log.Information("MeasureDevice {@IpAddress} -> Measuring data stored to log file {File}", dataId.IPAddress.ToString(), cpuDataStore.ToString());

                            // Store Data to Queue
                            while (lockSendingToApi)
                                Log.Information("MeasureDevice {@IpAddress} ->  Sending locked. Can not save data. Waiting for can save signal.");
                            lock(dataQueue)
                            {
                                dataQueue.Enqueue(sb.ToString());
                            }
                            sb.Clear();
                            Log.Information("MeasureDevice {@IpAddress} -> Measuring data stored in queue.", dataId.IPAddress.ToString());

                            // Prepare next measure
                            dataId.DataID = dataId.DataID + 1;
                        }
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
