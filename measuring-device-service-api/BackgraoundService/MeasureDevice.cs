﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MeasureDeviceServiceAPIProject.BackgraoundService;
using MeasureDeviceServiceAPIProject.Service;
using MeasureDeviceProject.Model;
using Serilog;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using System.IO;
using MeasureDeviceServiceAPIProject.Service.SendDataToServer;
using DataModel.MDDataModel;
using MeasureDeviceServiceAPIProject.APIService;
using DataModel.EFDataModel;

namespace MeasureDeviceProject.BackgraoundService
{
    public abstract class MeasureDevice : BackgroundService, IDisposable, IMeasureDevice
    {

        private readonly ILogger<MeasureDevice> logger;
        private readonly IConfiguration configuration;
        private string path=string.Empty;

        public MDIPAddress IPAddress { get; set; } = null;
       
        public MDState MDState { get; set; } = null;

        private MeasureStoreSystem msds=null;
        private SendBackupFileSystem sbfs = null;

        Thread thredPeridodically = null;
        Thread thredSendBackupFileSystem = null;

        CancellationToken myToken;

        private int measuringInterval = 1000;
        public int MeasuringInterval
        {
            get { return measuringInterval; }
            set
            {
                Log.Information("MeasureDevice {@IpAddress} -> New intaerval set: {interval}", IPAddress.ToString(), MeasuringInterval);
                measuringInterval = value;                
            }            
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }


        public MeasureDevice(IConfiguration configuration, ILogger<MeasureDevice> logger, MDIPAddress MDIPAddress, int id, int  measuringInterval)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.id = id;

            IPAddress= MDIPAddress;
            this.measuringInterval = measuringInterval;

            path = configuration.GetValue<string>("LogMeasurePath");
            Log.Information("MeasureDevice {@IpAddress} -> Path is {path}", IPAddress.ToString(), path);
            
            msds = new MeasureStoreSystem(logger, IPAddress,path,StorePeriod.EveryMinit);
            sbfs = new SendBackupFileSystem(logger, path + IPAddress.ToString());


            MDState = new MDState();
            MDState.MeasuringInterval = measuringInterval;
            MDState.StopMeasuring();
            MDState.StopWorking();

            msds.Stop();
            sbfs.Stop();
           
            thredPeridodically = new Thread(new ThreadStart(msds.StoringDataPeriodically));
            thredSendBackupFileSystem = new Thread(new ThreadStart(sbfs.Send));

            thredPeridodically.Priority = ThreadPriority.Lowest;
            thredSendBackupFileSystem.Priority = ThreadPriority.Lowest;
            thredPeridodically.Start();
            thredSendBackupFileSystem.Start();


        }

        public void StopMeasuring()
        {
            if (MDState.IsWorking && MDState.IsMeasuring)
            {
                if (msds != null)
                {
                    msds.Stop();
                    MDState.StopMeasuring();
                }
            }
        }

        public void StartMeasuring()
        {
            if (MDState.IsWorking && !MDState.IsMeasuring)
            {
                if (msds != null)
                {
                    msds.Start();
                    MDState.StartMeasuring();
                }
            }
        }

        public async override Task StartAsync(CancellationToken cancellationToken)
        {
            if (!MDState.IsWorking)
            {
                logger.LogInformation("MeasureDevice {IpAddress} -> StartAsync", IPAddress);
                logger.LogInformation("MeasureDevice {IpAddress} -> StartAsync, mesuring interval is {Interval}", IPAddress, measuringInterval);

                myToken = cancellationToken;

                if (myToken.IsCancellationRequested)
                {
                    logger.LogInformation("Token cancel is requested");
                }
                else
                {
                    logger.LogInformation("Token cancel is not requested");
                }

                // A device adatait elküljük a szerverbe, ott vagy új bejegyzésként, vagy frissítésként beíródik.
                MeasureDeviceAPIService mdAPI = new MeasureDeviceAPIService(logger);
                EFMeasureDevice device = new EFMeasureDevice(id,IPAddress.ToString(), measuringInterval);
                await mdAPI.SendMDDataToAsync(device);

                // Az eszközön a periódukos adat loggolást és az adatküldést engeélyezzük
                msds.Start();
                sbfs.Start();

                // Az ezsközt müködés állapotba hozzuk
                MDState.StartWorking();
                MDState.StartMeasuring();
                await base.StartAsync(cancellationToken);               
            }
            return;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("MeasureDevice {IpAddress} -> ExecuteAsync", IPAddress);
            if (myToken.IsCancellationRequested)
            {
                logger.LogInformation("ExecuteAsync Token cancel is requested");
            }
            else
            {
                logger.LogInformation("ExecuteAsync Token cancel is not requested");
            }

            while (!myToken.IsCancellationRequested)
            {
                
                logger.LogInformation("MeasureDevice {IpAddress}:  ExecuteAsync {time}", IPAddress, DateTimeOffset.Now.ToString("yyyy.MM.dd HH: mm:ss"));

                // CPU hőmérséklet mérés
                msds.MeasuringCPUUsage();

                await Task.Delay(TimeSpan.FromMilliseconds(measuringInterval), myToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            if (MDState.IsWorking)
            {
                logger.LogInformation("MeasureDevice {@IpAddress} -> StopAsync: {time}", DateTimeOffset.Now);
                // thredPeridodically.Abort();
                msds.Stop();
                //thredSendBackupFileSystem.Abort();
                sbfs.Stop();

                myToken = cancellationToken;
                return base.StopAsync(myToken);
            }
            else
                return Task.CompletedTask;
        }

        public override void Dispose()
        {
            if (msds != null)
            {
                msds.Dispose();
            }
            if (sbfs!= null)
            {
                sbfs.Dispose();
            }
            if (MDState!=null)
            {
                MDState.Dispose();
            }
            base.Dispose();
        }
    }
}

