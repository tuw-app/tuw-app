using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceServiceAPIProject.APIService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MeasureDeviceServiceAPIProject.Service.SendDataToServer
{

    public class SendBackupFileSystem : IDisposable
    {
        ILogger<MeasureDevice> logger = null;

        private string path;
        public string Path { get => path; set => path = value; }

        public SendBackupFileSystem(ILogger<MeasureDevice> logger, string path)
        {
            this.logger = logger;
            this.path = path;
        }

        public void Init()
        { 
        }

        public void Send()
        {
            CPUAPIService api=new CPUAPIService();
            logger.LogInformation("SendBackupFileSystem -> Send Started");
            while (true)
            {
                List<string> backupFiles = GetBackupFiles(path);
                if (backupFiles.Count!=0)
                {
                    logger.LogInformation("SendBackupFileSystem -> There are {Count} backup file.", backupFiles.Count);
                    foreach (string backupFile in backupFiles) 
                    {
                        List<string> bug = null;
                        while (bug != null)
                        {
                            bug = new List<string>();
                            List<string> lines = File.ReadAllLines(path + "\\" + backupFile).ToList();
                            var task = lines.Select(async line =>
                            {
                                await api.SendNewCPUDataAsync(line);
                                bug.Add(line);
                            }
                            );
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
            string[] files = Directory.GetFiles(path, "*.bak");
            List<string> fileNames = new List<string>();
            if (files.Length == 0)
            {
                return fileNames;
            }
            else
            {
                return files.ToList();
            }
        }

        public void Dispose()
        {

        }
    }
}
