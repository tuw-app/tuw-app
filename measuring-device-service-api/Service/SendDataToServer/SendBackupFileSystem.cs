using MeasureDeviceServiceAPIProject.APIService;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MeasureDeviceServiceAPIProject.Service.SendDataToServer
{

    public class SendBackupFileSystem
    {
        private string path;
        public string Path { get => path; set => path = value; }

        public SendBackupFileSystem(string path)
        {
            this.path = path;
        }

        public void init()
        { 
        }

        public async void send()
        {
            CPUAPIService api=new CPUAPIService();
            while(true)
            {
                List<string> backupFiles = GetBackupFiles(path);
                if (backupFiles.Count!=0)
                {
                    foreach(string backupFile in backupFiles) 
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
                                bug = null;
                        }                        
                    }
;
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
    }
}
