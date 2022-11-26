using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;

using System;
using System.Collections.Generic;

using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using MeasureDeviceProject.Service.CPUUsage;
using MeasureDeviceServiceAPIProject.Service.PeriodicallyStore;
using MeasureDeviceProject.Model.CPUUsageModel;

namespace MeasureDeviceServiceAPIProject.Service
{
    public class MeasureStoreSystem : IMeasureStoreSystem, IDisposable
    {
        StorePeriod storePeriod = StorePeriod.EveryMinit;

        ILogger<MeasureDevice> logger;

        //private bool lockMesuring = false;
        //private bool lockSendingToApi = true;
        private bool stopDevice = false;

        private MDIPAddress IPAddress { get; }

        private string path = string.Empty;

        private Queue<MeasuredCPUUsage> measuredCPUUsageQeueue = new Queue<MeasuredCPUUsage>();
     
        public MeasureStoreSystem(ILogger<MeasureDevice> logger, MDIPAddress IPAddress, string path, StorePeriod storePeriod = StorePeriod.EveryMinit )
        {
            this.logger = logger;

            this.IPAddress = IPAddress;
            this.storePeriod = storePeriod;
            this.path = path;

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
                Log.Information("MeasureDevice {@IpAddress} -> Initialize device...", IPAddress.ToString());
                //https://stackoverflow.com/questions/53727850/how-to-run-backgroundservice-on-a-timer-in-asp-net-core-2-1

                stopDevice = false;
                //lockMesuring = false;
                //lockSendingToApi = false;
            }

        }

        public async void MeasuringCPUUsage()
        {
            CPUUsageService cpuMeasuring = new CPUUsageService();
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
                    //Log.Information("MeasureDevice {@IpAddress} -> Measuring data: {Data}", IPAddress.ToString(), cpuMeasuring.GetCPUUsageToLog());

                    lock (measuredCPUUsageQeueue)
                    {
                        MeasuredCPUUsage measuredCPUUsag = new MeasuredCPUUsage(cpuMeasuring.UsageResult, measuringTime);
                        measuredCPUUsageQeueue.Enqueue(measuredCPUUsag);
                        Log.Information("MeasureDevice {@IpAddress} -> Measuring result {Result} is added to queue", IPAddress.ToString(), measuredCPUUsag.CPUUsageResult);
                    }
                }
            }
        }

        public void StoringDataPeriodically()
        {
            PeriodicallyStoreSystem cpuDataStorePeriodically = null;
            using (LogContext.PushProperty(IPAddress.ToString(), 1))
            {
                MeasuredCPUUsage mesuredResult = null;
                MDStoreFileId storeFileId = null;

                // init
                Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Init", IPAddress.ToString());
                Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Init->Path is {path}", IPAddress.ToString(), path);
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
                    Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Init -> New sotre file id {Id}", IPAddress.ToString(), storeFileId.GetMeasruringPeriodicFileName);

                    if (cpuDataStorePeriodically == null)
                    {
                        Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Init -> No CPU data store periodically", IPAddress.ToString());
                        cpuDataStorePeriodically = new PeriodicallyStoreSystem(logger,IPAddress, storeFileId, path);
                        Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Init -> New CPU data store {DataStore}", IPAddress.ToString(), cpuDataStorePeriodically);

                    }
                }

                if (cpuDataStorePeriodically.IsFileExsist())
                {
                    ulong exsistingID = cpuDataStorePeriodically.GetLastLineId();
                    cpuDataStorePeriodically.SetDataId(mesuredResult.MeasureTime,exsistingID);                    
                }
                else
                {
                    cpuDataStorePeriodically.SetDataId(mesuredResult.MeasureTime, 1);
                }
                Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Init -> First Data Id is: {Id}", IPAddress.ToString(), cpuDataStorePeriodically.GetDataIdToLog());
                while (true)
                {
                    while (measuredCPUUsageQeueue.Count == 0)
                    {
                        //Log.Information("MeasureDevice {@IpAddress} -> StoringDataPeriodically->Sending system -> No mesured data in queue", IPAddress.ToString());
                    }
                    lock (measuredCPUUsageQeueue)
                    {
                        // kivesszük a következő tárolandó elemet
                        mesuredResult = measuredCPUUsageQeueue.Dequeue();
                    }
                    lock (mesuredResult)
                    {
                        // Az új tárolandó adat mérés időpontja alapján meghatározzuk, hogy melyik fájlba kerül az adat
                        cpuDataStorePeriodically.DetermineTheStoreFile(mesuredResult);

                        // Store Data to log file                              
                        MeasuredCPUDataStore measuredData = new MeasuredCPUDataStore(cpuDataStorePeriodically.GetDataId(),mesuredResult);
                        //Log.Information("MeasureDevice {@IpAddress} -> Data to store:", IPAddress.ToString(), measuredData.MeasuredCPUDataToStore);
                        try
                        {
                            cpuDataStorePeriodically.WriteData(measuredData.MeasuredCPUDataToStore);
                            Log.Information("MeasureDevice {@IpAddress} -> {Data} stored.", IPAddress.ToString(), measuredData.MeasuredCPUDataToStore);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("MeasureDevice {@IpAddress} -> Measuring data store faild. {Message}", IPAddress.ToString(), ex.Message);
                        }

                        //Log.Information("MeasureDevice {@IpAddress} -> Measuring data stored to log file {File}", IPAddress.ToString(), cpuDataStorePeriodically.StoredFileId.GetMeasruringPeriodicFileName);

                        // Prepare to new DataId
                        cpuDataStorePeriodically.IncrementDataId();
                    }
                }
            }
        }
    
        
        public void Dispose()
        {
                               
        }
    }
}
