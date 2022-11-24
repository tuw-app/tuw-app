using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Permissions;
using System.Text;
using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using Microsoft.Extensions.Logging;

namespace MeasureDeviceProject.Service.FileWriter
{


    public class MeasuringDataStore : IDisposable
    {
        ILogger<MeasureDevice> logger;

        private string path=string.Empty;
        private string fileName=string.Empty;

        private DateTime CurrentStoreTime { get; set; }
      

        FileStream currentStream = null;
        StreamWriter sw = null;

        public string FileName { get; set; }

        private string FullFileName { get { return path + fileName; } }

        
        public MeasuringDataStore(ILogger<MeasureDevice> logger,  string path, string fileName)
        {
            this.logger = logger;
            this.path = path;
            this.fileName = fileName;
        }

        private void Init()
        {
            // https://zetcode.com/csharp/filestream/
            if (!File.Exists(FileName))
            {
                // Új fájl létrehozása
                try
                {
                    currentStream = new FileStream(FullFileName, FileMode.CreateNew, FileAccess.Write);
                    sw = new StreamWriter(currentStream);
                   
                    logger.LogInformation("MeasuringDataStore {FileName} -> File not exsist. File created.", FileName);
                }
                catch (Exception ex)
                {
                    logger.LogError("MeasuringDataStore {FileName} -> Can not create file to write. Exception:{Message}", FileName, ex.Message);
                    throw new MeasuringDataStoreException($"Can not create file {FileName} to write.\n{ex.Message}");
                }
            }
            else
            {
                // A fájl már létezik, megnyitjuk
                try
                {
                    currentStream = new FileStream(FullFileName, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(currentStream, Encoding.UTF8, 65536);

                    logger.LogInformation("MeasuringDataStore {FileName} -> Open file to append data.", FileName);
                }
                catch (Exception ex)
                {
                    logger.LogError("MeasuringDataStore {FileName} -> Can not open file  to append. Exception:{Message}", FileName, ex.Message);
                    throw new MeasuringDataStoreException($"Can not open file {FileName} to append.\n{ex.Message}");
                }                
            }
        }
        /*
        public void ChangeLoggintToNextDay()
        {
            logger.LogInformation("MeasuringDataStore {FileName} -> Change logging to next day", FileName);
            Close();
            Init();
        }
        */
        public void WriteData(string data)
        {
            if (currentStream == null || sw == null)
            {
                Init();
            }
            else
            {
                try
                {
                    sw.WriteLine(data);
                    logger.LogInformation("MeasuringDataStore {FileName} -> Data is stored: {Data}", FileName,data);
                }
                catch (Exception ex)
                {
                    logger.LogError("MeasuringDataStore {FileName} -> Can not write to file {Message}", FileName,ex.Message);
                    throw new MeasuringDataStoreException($"Can not write to file {FileName}.\n{ex.Message}");
                }
            }
        }

        public void Close()
        {
            if (sw!=null)
                sw.Close();
            if (currentStream != null)
                currentStream.Close();
            logger.LogInformation("MeasuringDataStore {FileName} -> File is closed", FileName);
        }

        public void Dispose()
        {
            if (sw != null)
            {
                sw.Close();
                sw.Dispose();
            }
            if (currentStream != null)
            {
                currentStream.Close();
                currentStream.Dispose();
            }
        }

        public string GetId { get { return ToString();  } }

        public override string ToString()
        {
            return FileName;
        }

        public void ChangeFileExtension()
        {
            string newFileName = FullFileName.Replace(".txt", ".bak");
            try
            {
                File.Move(FullFileName, newFileName);
            }
            catch
            {
            }

        }
    }
}
