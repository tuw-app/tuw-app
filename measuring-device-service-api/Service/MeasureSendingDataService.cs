using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
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

        private MDDataId dataId=null;

        private bool lockMesuring = false;
        private bool lockSendingToApi = true;
        private bool stopDevice = false;

        CPUUsageService cpuMeasuring = null;
        MeasuringDataStore cpuDataStore = null;
        MDStoreFileId storeFileId = null;

        private MDIPAddress IPAddress { get; }

        private string path = string.Empty;

        private Queue<MesuredCPUUsage> measuredCPUUsageQeueue = new Queue<MesuredCPUUsage>();
        private DataPerFile storedDataPerFile=null;



        public MeasureSendingDataService(IConfiguration configuration, ILogger<MeasureDevice> logger,  MDIPAddress IPAddress, StorePeriod storePeriod = StorePeriod.EveryMinit)
        {
            this.logger = logger;
            this.configuration = configuration;
            
            this.IPAddress=IPAddress;                        
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

        /*    
         *                            // Mesuring data storing
                           // Measuring file name
                           // Tárolás periódusának meghatározása -> tároló fájl név!                        
                           // Honnan tudom, hogy egy fájlba már minden adat be van írva?
                           // Egy Dictionaryban nyilvántartom melyik fájlba hány adatot írtam.
         *    
         *    if (storeFileId == null)
                           {
                               // Induláskor meghatározzuk az első tárolandó fájl nevét.
                               storeFileId = new MDStoreFileId(measuringTime, storePeriod);
           cpuDataStore = new MeasuringDataStore(logger, path, storeFileId.GetMeasruringPeriodicFileName);

           Log.Information("MeasureDevice {@IpAddress} -> New File id: {StoreFileID}", IPAddress.ToString(), storeFileId.GetMeasruringPeriodicFileName);
                           }

                       lock (storeFileId)
                       { 
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
                           {   // Prepera mew data id 
                               dataId = new MDDataId(IPAddress, measuringTime);
                               Log.Information("MeasureDevice {@IpAddress} -> New data id: {DataID}", IPAddress.ToString(), dataId.ToString());
                           }


                       //    { 
                               dataId.DateTime = measuringTime;
                               Log.Information("MeasureDevice {@IpAddress} -> Data is is lockd: {DataID}", IPAddress.ToString(), dataId.ToString());
                               // Store Data to log file                              
                               MeasuredDataStore measuredData = new MeasuredDataStore(dataId.GetId, cuMeasuring.GetCPUUsage());
                               Log.Information("MeasureDevice {@IpAddress} -> Data to store in file:", IPAddress.ToString(), measuredData.MeasuredData);
                               try
                               {
                                   cpuDataStore.WriteData(measuredData.MeasuredData);
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
                                   dataQueue.Enqueue(measuredData.MeasuredData);
                               }
                               Log.Information("MeasureDevice {@IpAddress} -> Measuring data stored in queue.", IPAddress.ToString());
                               // Prepare to new DataId
                               dataId.IncrementDataId();
                           }
                       }
                   }
               }
           }
        */
        public void Dispose()
        {
            if (cpuMeasuring!=null)
                cpuMeasuring.Dispose();
                                
        }
    }
}
