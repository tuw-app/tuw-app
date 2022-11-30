using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceServiceAPIProject.APIService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MeasureDeviceServiceAPIProject.Service.SendDataToServer
{

    public class SendBackupFileSystem : IDisposable
    {
        ILogger<MeasureDevice> logger = null;

        private string path;
        public string Path { get => path; set => path = value; }

        private bool stop = false;

        public SendBackupFileSystem(ILogger<MeasureDevice> logger, string path)
        {
            this.logger = logger;
            this.path = path;
        }

        public void Stop()
        { 
            stop=true;
         }

        public void Start()
        {
            stop = false;
        }

        public async void Send()
        {
            CPUAPIService api =new CPUAPIService(logger);
            logger.LogInformation("SendBackupFileSystem -> Send Started");
            while (true)
            {
                if (stop)
                {
                    logger.LogInformation("SendBackupFileSystem -> Stop");
                    return;
                }
                List<string> backupFiles = GetBackupFiles(path);
                if (backupFiles.Count!=0)
                {
                    //logger.LogInformation("SendBackupFileSystem -> There are {Count} backup file.", backupFiles.Count);
                    foreach (string backupFile in backupFiles) 
                    {
                        logger.LogInformation("SendBackupFileSystem -> Precessed file:{file}.", backupFile);
                        List<string> bug = new List<string>();
                        while (bug != null)
                        {
                            if (stop)
                            {
                                logger.LogInformation("SendBackupFileSystem -> Stop");
                                return;
                            }
                            bug = new List<string>();
                            List<string> lines = File.ReadAllLines(backupFile).ToList();
                            logger.LogInformation("SendBackupFileSystem -> Number of line in file {Count}.", lines.Count);
                            /*var task = lines.Select(async line =>
                            {
                                HttpStatusCode code=await api.SendNewCPUDataAsync(line);
                                logger.LogInformation("SendBackupFileSystem -> Status code after sending {Code}.", code);
                                if (code!=HttpStatusCode.OK)
                                    bug.Add(line);
                            }
                            );*/
                            foreach(string line in lines)
                            {
                                HttpStatusCode code = await api.SendNewCPUDataAsync(line);
                                logger.LogInformation("SendBackupFileSystem -> Status code after sending {Code}.", code);
                                if (code != HttpStatusCode.OK)
                                    bug.Add(line);

                            }
                            if (bug.Count == 0)
                            {
                                logger.LogInformation("SendBackupFileSystem -> All line from {File} is sended.", backupFile);
                                bug = null;
                                File.Move(backupFile, backupFile.Replace("bak", "tmp"));
                            }
                            else
                            {
                                logger.LogInformation("SendBackupFileSystem -> From {File} {Count} data is not sended.", backupFile, bug.Count);
                            }
                        }                        
                    }
                }
            }
        }


        public List<string> GetBackupFiles(string path)
        {
            try
            {
                string[] files = null;
                files = Directory.GetFiles(path, "*.bak");

                if (files == null || files.Length == 0)
                {
                    // nincsenek .bak fájlok
                    return files.ToList();
                }
                else
                {
                    logger.LogInformation("SendBackupFileSystem -> GetBackupFiles->Found {count} backup file.", files.Length);
                    List<string> fileNames = new List<string>();
                    return files.ToList();
                }
            }
            catch
            {
                return new List<string>();
            }
        }

        public void Dispose()
        {

        }
    }
}
