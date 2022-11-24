using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using MeasureDeviceProject.Model.MeasureElements;
using MeasureDeviceProject.Service.CPUUsage;
using MeasureDeviceProject.Service.FileWriter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.IO;
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

        private bool lockMesuring = false;
        private bool lockSendingToApi = true;
        private bool stopDevice = false;

        CPUUsageService cpuMeasuring = null;
        PeriodicallyStoreSystem cpuDataStorePeriodically = null;

        private MDIPAddress IPAddress { get; }

        private string path = string.Empty;

        private static Queue<MesuredCPUUsage> measuredCPUUsageQeueue = new Queue<MesuredCPUUsage>();
     
        public MeasureSendingDataService(IConfiguration configuration, ILogger<MeasureDevice> logger, MDIPAddress IPAddress, StorePeriod storePeriod = StorePeriod.EveryMinit)
        {
            this.logger = logger;
            this.configuration = configuration;

            this.IPAddress = IPAddress;
            this.storePeriod = storePeriod;

            Initialize();
        }

        public void Start()
        {
            stopDevice = false;
        }

        public void Stop()
        {

            stopDevice = true;
        }

        private void Initialize()
        {
            using (LogContext.PushProperty(IPAddress.ToString(), 1))
            {
                Log.Information("MeasureDevice {@IpAddress} -> Initialize device...", IPAddress);
                //https://stackoverflow.com/questions/53727850/how-to-run-backgroundservice-on-a-timer-in-asp-net-core-2-1

                cpuMeasuring = new CPUUsageService();
                stopDevice = false;
                lockMesuring = false;
                lockSendingToApi = false;

                //path = configuration["Path"]; // ???????
                path = "f:\\tuw\\log\\";
                Log.Information("MeasureDevice {@IpAddress} -> The log path is {Path}", IPAddress, path);
            }

        }

        public async void MeasuringCPUUsage()
        {
            using (LogContext.PushProperty(IPAddress.ToString(), 1))
            {
                Log.Information("MeasureDevice {IpAddress} -> Measuring data: begin working.", IPAddress.ToString());
                if (stopDevice)
                {
                    Log.Information("MeasureDevice {@IpAddress} ->  Device is stoped.", IPAddress.ToString());
                    return;
                }
                else
                {
                    Log.Information("MeasureDevice {@IpAddress} -> prepare to measuring.", IPAddress.ToString());
                    // Mesuring
                    await cpuMeasuring.ReadCPUUsage();
                    DateTime measuringTime = DateTime.Now;

                    Log.Information("MeasureDevice {@IpAddress} -> Measuring time: {Time}", IPAddress.ToString(), measuringTime.ToString("yyyy.MM.dd HH:mm:ss)"));
                    Log.Information("MeasureDevice {@IpAddress} -> Measuring data: {Data}", IPAddress.ToString(), cpuMeasuring.GetCPUUsageToLog());

                    MesuredCPUUsage measuredCPUUsag = new MesuredCPUUsage(cpuMeasuring.UsageResult, measuringTime);

                    lock (measuredCPUUsageQeueue)
                    {
                        measuredCPUUsageQeueue.Enqueue(measuredCPUUsag);
                        Log.Information("MeasureDevice {@IpAddress} -> Measuring result is added to queue", IPAddress.ToString());
                    }
                }
            }
        }

        public void StoringDataPeriodically()
        {
            using (LogContext.PushProperty(IPAddress.ToString(), 1))
            {
                MesuredCPUUsage mesuredResult = null;
                MDStoreFileId storeFileId = null;

                // init
                while (measuredCPUUsageQeueue.Count == 0)
                {
                    //Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Init -> No mesured data in queue. {Count}", IPAddress.ToString(), measuredCPUUsageQeueue.Count);
                }
                lock (measuredCPUUsageQeueue)
                {
                    // kivasszük az első adatot
                    mesuredResult = measuredCPUUsageQeueue.Peek();
                    Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Init -> Peek first data: {data}", IPAddress.ToString(), mesuredResult);
                }
                
                // A következő adattal elkezdjük a periódikus adat tárolást
                Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Init -> First mesured data: {data}", IPAddress.ToString(), mesuredResult);

                if (storeFileId == null)
                {
                    // Meghatározzuk az első adat id-jét és létrehozzuk a periódikus adat taároló rendszert
                    Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Init -> No sotre file id", IPAddress.ToString());
                    storeFileId = new MDStoreFileId(mesuredResult.MeasureTime, storePeriod);
                    Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Init -> New sotre file id {Id}", IPAddress.ToString(), storeFileId);

                    if (cpuDataStorePeriodically == null)
                    {
                        Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Init -> No CPU data store periodically", IPAddress.ToString());
                        cpuDataStorePeriodically = new PeriodicallyStoreSystem(logger, storeFileId, path);
                        Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Init -> New CPU data store periodically", IPAddress.ToString(), cpuDataStorePeriodically);

                    }
                }

                if (cpuDataStorePeriodically.IsFileExsist())
                {
                    ulong exsistingID = cpuDataStorePeriodically.GetLastLineId();
                    cpuDataStorePeriodically.MDDataId.DataID = exsistingID;
                }
                else
                {
                    cpuDataStorePeriodically.MDDataId.DataID = 1;
                }
                Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Init -> First Data Id is: {Id}", IPAddress.ToString(), cpuDataStorePeriodically.MDDataId.DataID);
                while (true)
                {
                    if (measuredCPUUsageQeueue.Count == 0)
                    {
                        Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Sending system -> No mesured data in queue", IPAddress.ToString());
                    }
                    // kivesszük a következő tárolandó elemet
                    mesuredResult = measuredCPUUsageQeueue.Dequeue();

                    lock (mesuredResult)
                    {
                        // Meghatározzuk az új tárolandó elem file ID-jét
                        cpuDataStorePeriodically.StoredFileId = new MDStoreFileId(mesuredResult.MeasureTime, storePeriod);
                        Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Init -> New sotre file id {Id}", IPAddress.ToString(), storeFileId);

                        // Az új tárolandó adat mérés időpontja alapján meghatározzuk, hogy melyik fájlba kerül az adat
                        cpuDataStorePeriodically.DetermineTheStoreFile(mesuredResult);


                        // Store Data to log file                              
                        MeasuredCPUDataStore measuredData = new MeasuredCPUDataStore(cpuDataStorePeriodically.MDDataId,mesuredResult);
                        Log.Information("MeasureDevice {@IpAddress} -> Data to store in file:", IPAddress.ToString(), measuredData.MeasuredCPUDataToStore);
                        try
                        {
                            cpuDataStorePeriodically.WriteData(measuredData.MeasuredCPUDataToStore);
                            Log.Information("MeasureDevice {@IpAddress} -> Measuring data stored in txt file.", IPAddress.ToString());
                        }
                        catch (Exception ex)
                        {
                            Log.Error("MeasureDevice {@IpAddress} -> Measuring data store faild. {Message}", IPAddress.ToString(), ex.Message);
                        }

                        Log.Information("MeasureDevice {@IpAddress} -> Measuring data stored to log file {File}", IPAddress.ToString(), cpuDataStorePeriodically.StoredFileId.GetMeasruringPeriodicFileName);

                        // Prepare to new DataId
                        cpuDataStorePeriodically.MDDataId.IncrementDataId();
                    }
                }
            }
        }
    
        
        public void Dispose()
        {
            if (cpuMeasuring!=null)
                cpuMeasuring.Dispose();
                                
        }
    }
}
