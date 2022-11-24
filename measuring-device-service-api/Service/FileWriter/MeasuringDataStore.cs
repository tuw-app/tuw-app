using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using Microsoft.Extensions.Logging;

namespace MeasureDeviceProject.Service.FileWriter
{
    public class MeasuringDataStore : IDisposable
    {
        ILogger<MeasureDevice> logger;

        private string path;
        private string fileName;
        private MDDataId dataId;

        private DateTime CurrentStoreTime { get { return DateTime.Now; } }
        private string DateTimeExtenstion { get { return CurrentStoreTime.ToString("-yyyy-MM-dd"); } }
        private string StoredDataType { get; set; }

        private int CurrentStoreDay { get; set; }

        FileStream currentStream = null;
        StreamWriter sw = null;

        private string FileName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(path).Append(fileName).Append(DateTimeExtenstion).Append(StoredDataType).Append(".txt");
                return sb.ToString();
            }
            
        }

        string dateTimeFileExcension = string.Empty;

        public MeasuringDataStore(ILogger<MeasureDevice> logger, string storedDataType, string path, string fileName, MDDataId dataId)
        {
            this.logger = logger;

            this.StoredDataType = storedDataType;
            this.path = path;
            this.fileName = fileName;
            this.dataId = dataId;    

        }

        private void Init()
        {
            // https://zetcode.com/csharp/filestream/
            if (!File.Exists(FileName))
            {
                try
                {
                    currentStream = new FileStream(FileName, FileMode.CreateNew, FileAccess.Write);
                    //currentStream.Close();
                    //currentStream = new FileStream(FileName, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(currentStream);

                    CurrentStoreDay = DateTime.Now.Day;
                    logger.LogInformation("MeasuringDataStore {FileName} -> File not exsist. File created. Current StoreDay: {StoreDay}", FileName, CurrentStoreDay.ToString());
                }
                catch (Exception ex)
                {
                    logger.LogError("MeasuringDataStore {FileName} -> Can not create file to write. Exception:{Message}", FileName, ex.Message);
                    throw new MeasuringDataStoreException($"Can not create file {FileName} to write.\n{ex.Message}");
                }
            }
            else
            {
                try
                {
                    currentStream = new FileStream(FileName, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(currentStream, Encoding.UTF8, 65536);

                    CurrentStoreDay = DateTime.Now.Day;
                    logger.LogInformation("MeasuringDataStore {FileName} -> Open file to adding. Current StoreDay: {StoreDay}", FileName, CurrentStoreDay.ToString());
                }
                catch (Exception ex)
                {
                    logger.LogError("MeasuringDataStore {FileName} -> Can not open file  to write. Exception:{Message}", FileName, ex.Message);
                    throw new MeasuringDataStoreException($"Can not open file {FileName} to write.\n{ex.Message}");
                }                
            }
        }

        public void ChangeLoggintToNextDay()
        {
            logger.LogInformation("MeasuringDataStore {FileName} -> Change logging to next day", FileName);
            Close();
            Init();
        }

        public void WriteData(string data)
        {
            // Mi van ha már másik nap van?
            if (DateTime.Now.Day!= CurrentStoreDay)
            {
                ChangeLoggintToNextDay();
            }
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

        public override string ToString()
        {
            return FileName;
        }
    }
}
