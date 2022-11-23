using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

using MeasureDeviceProject.Model;

namespace MeasureDeviceProject.Service.FileWriter
{
    public class MeasuringDataStore : IDisposable
    {
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
                sb.Append(path).Append(fileName).Append(DateTimeExtenstion).Append(StoredDataType).Append(dataId.IPAddress).Append(".txt");
                return sb.ToString();
            }
            
        }

        string dateTimeFileExcension = string.Empty;

        public MeasuringDataStore(string storedDataType, string path, string fileName, MDDataId dataId)
        {
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
                }
                catch (Exception ex)
                {
                    throw new MeasuringDataStoreException($"Can not open file {FileName} to write.\n{ex.Message}");
                }
            }
            else
            {
                try
                {
                    currentStream = new FileStream(FileName, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(currentStream, Encoding.UTF8, 65536);

                    CurrentStoreDay = DateTime.Now.Day;
                }
                catch (Exception ex)
                {
                    throw new MeasuringDataStoreException($"Can not open file {FileName} to write.\n{ex.Message}");
                }                
            }
        }

        public void ChangeToNewFile()
        {
            Close();
            Init();
        }

        public void WriteData(string data)
        {
            // Mi van ha már másik nap van?
            if (DateTime.Now.Day!= CurrentStoreDay)
            {
                ChangeToNewFile();
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
                }
                catch (Exception ex)
                {
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
