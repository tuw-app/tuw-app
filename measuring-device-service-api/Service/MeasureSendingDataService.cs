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
        StorePeriod storePeriod = StorePeriod.EveryMinit;

        ILogger<MeasureDevice> logger;
        IConfiguration configuration;

        private double measureingInterval = 0;
        private MDDataId dataId=null;

        private bool lockMesuring = false;
        private bool lockSendingToApi = true;
        private bool stopDevice = false;

        private Timer timer = null;
        CPUUsageService cuMeasuring = null;
        MeasuringDataStore cpuDataStore = null;
        MDStoreFileId storeFileId = null;

        private MDIPAddress IPAddress { get; }

        private string path = string.Empty;

        private Queue<string> dataQueue = new Queue<string>();
        private DataPerFile storedDataPerFile=null;

        public void SetMeasureingInterval(double measuringInterval)
        {
            this.measureingInterval = measuringInterval;
        }

        public MeasureSendingDataService(IConfiguration configuration, ILogger<MeasureDevice> logger,  MDIPAddress IPAddress, double MeasureingInterval, StorePeriod storePeriod = StorePeriod.EveryMinit)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.measureingInterval = MeasureingInterval;
            
            this.IPAddress=IPAddress;                        
            this.storePeriod = storePeriod;

            Initialize();
        }

        public void Start()
        {
            if (timer != null)
            {
                timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(measureingInterval));
            }
            stopDevice = false;
        }

        public void Stop()
        {
            if (timer != null)
                timer.Change(Timeout.Infinite, 0);
            stopDevice = true;
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
            using (LogContext.PushProperty(IPAddress.ToString(), 1))
            {
                Log.Information("MeasureDevice {@IpAddress} -> Initialize device...", IPAddress);
                //https://stackoverflow.com/questions/53727850/how-to-run-backgroundservice-on-a-timer-in-asp-net-core-2-1

                cuMeasuring = new CPUUsageService();
                stopDevice = false;
                lockMesuring = false;
                lockSendingToApi = false;

                path = configuration["Path"]; // ???????
                Log.Information("MeasureDevice {@IpAddress} -> The log path is {Path}", IPAddress, path);
                
                // Miért nem működik 
                timer = new Timer(MeasuringData, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(measureingInterval));
            }

        }

        private async void MeasuringData(object state)
        {
            using (LogContext.PushProperty(IPAddress.ToString(), 1))
            {
                Log.Information("MeasureDevice {IpAddress} -> Measuring data: begin working.", IPAddress.ToString());
                while (true)
                {
                    if (stopDevice)
                    {
                        Log.Information("MeasureDevice {@IpAddress} ->  Device is stoped.", IPAddress.ToString());
                    }
                    else
                    {
                        Log.Information("MeasureDevice {@IpAddress} -> prepare to measuring.", IPAddress.ToString());
                        // Mesuring
                        await cuMeasuring.ReadCPUUsage();                                               
                        DateTime measuringTime = DateTime.Now;

                        Log.Information("MeasureDevice {@IpAddress} -> Measuring time: {Time}", IPAddress.ToString(), measuringTime.ToString("yyyy.MM.dd HH:mm:ss.ff)"));
                        Log.Information("MeasureDevice {@IpAddress} -> Measuring data: {Data}", IPAddress.ToString(), cuMeasuring.GetCPUUsageToLog());

                        // Mesuring data storing
                        // Measuring file name
                        // Tárolás periódusának meghatározása -> tároló fájl név!                        
                        // Honnan tudom, hogy egy fájlba már minden adat be van írva?
                        // Egy Dictionaryban nyilvántartom melyik fájlba hány adatot írtam.

                        if (storeFileId == null) 
                        {
                             // Induláskor meghatározzuk az első tárolandó fájl nevét.
                             storeFileId = new MDStoreFileId(measuringTime, storePeriod);
                             cpuDataStore = new MeasuringDataStore(logger, path , storeFileId.GetMeasruringPeriodicFileName);

                             Log.Information("MeasureDevice {@IpAddress} -> New File id: {StoreFileID}", IPAddress.ToString(),storeFileId.GetMeasruringPeriodicFileName);
                        }
                        if (!storeFileId.IsTheMesureTimeStampGood(measuringTime))
                        {
                            // Ha a periódus idő lejárt eltároljuk az adatok számát és meghatározzuk az új fájl nevét.
                            if (storedDataPerFile==null)
                            {
                                // Létrehozzuk a dictionary-t a fájl nevek és benne tárolt adatok tárolására
                                storedDataPerFile = new DataPerFile();
                            }
                            // Eltároljuk a fájl nevét és abban tárolt adatok számát amibe beírtuk az utolsó adatot
                            storedDataPerFile.Add(storeFileId.GetMeasruringPeriodicFileName, dataId.DataID);
                            // A fájlt zárjuk???
                            cpuDataStore.Close();
                            
                            Log.Information("MeasureDevice {@IpAddress} -> Number of open file: {OpenFileNumer}", IPAddress.ToString(), storedDataPerFile.NumberOfOpenFile.ToString());             
                            // A dataId-t visszaállítjuk
                            dataId.DataID = 1;
                            // Meghatározzuk az új fájl nevét és fájl írót
                            storeFileId.SetActulMeasureFileTimeStamp(measuringTime);
                            cpuDataStore.FileName = storeFileId.GetMeasruringPeriodicFileName;
                            

                            Log.Information("MeasureDevice {@IpAddress} -> New File id: {StoreFileID}", IPAddress.ToString(),storeFileId.GetMeasruringPeriodicFileName);
                        }
                        if (dataId == null)
                            dataId = new MDDataId(IPAddress, measuringTime);
                        lock (dataId)
                        {
                            // Store Data to log file
                            // Prepera data id 
                            if (dataId == null)
                                dataId = new MDDataId(IPAddress, measuringTime);
                            else
                            {
                                dataId.DateTime = measuringTime;
                                dataId.IncrementDataId();
                            }
                            MeasuredDataStore measureData = new MeasuredDataStore(dataId.GetId, cuMeasuring.GetCPUUsage());
                            try
                            {
                                cpuDataStore.WriteData(measureData.MeasuredData);
                                Log.Information("MeasureDevice {@IpAddress} -> Measuring data stored in logfile.", IPAddress.ToString());
                            }
                            catch (Exception ex)
                            {
                                Log.Error("MeasureDevice {@IpAddress} -> Measuring write data: {Data}", IPAddress.ToString(), cuMeasuring.GetCPUUsageToLog());
                            }

                            Log.Information("MeasureDevice {@IpAddress} -> Measuring data stored to log file {File}", IPAddress.ToString(), cpuDataStore.FileName);

                            // Store Data to Queue
                            while (lockSendingToApi)
                                Log.Information("MeasureDevice {@IpAddress} ->  Sending locked. Can not save data. Waiting for can save signal.");
                            lock (dataQueue)
                            {
                                dataQueue.Enqueue(measureData.MeasuredData);
                            }
                            Log.Information("MeasureDevice {@IpAddress} -> Measuring data stored in queue.", IPAddress.ToString());                            
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
