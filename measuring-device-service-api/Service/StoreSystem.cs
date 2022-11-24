using MeasureDeviceProject.BackgraoundService;
using MeasureDeviceProject.Model;
using MeasureDeviceProject.Service.FileWriter;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MeasureDeviceServiceAPIProject.Service
{
    public class PeriodicallyStoreSystem
    {
        ILogger<MeasureDevice> logger = null;

        MDIPAddress IPAddress = null;
        private MDStoreFileId storedFileId=null;
        private MeasuringDataStore measuringDataStore=null;
        private MDDataId mdDataId = null;

        private string path;
        public string Path
        {
            get { return path; }
        }

        private string FullPathFileName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                return sb.Append(path).Append(storedFileId.GetMeasruringPeriodicFileName).ToString();
            }
        }

        public PeriodicallyStoreSystem(ILogger<MeasureDevice> logger, MDIPAddress IPAddress, MDStoreFileId storedFileId, string path )
        {
            this.storedFileId = storedFileId;
            this.IPAddress = IPAddress;
            this.logger = logger;
            this.path = path;
            System.IO.Directory.CreateDirectory(path+IPAddress);
            measuringDataStore = new MeasuringDataStore(logger, IPAddress, path, storedFileId.GetMeasruringPeriodicFileName);
        }

        public void SetDataId( DateTime mesuringDataTime, ulong dataId)
        {
            if (mdDataId == null)
            {
                mdDataId = new MDDataId(IPAddress, mesuringDataTime, dataId);
            }
            else
            {
                mdDataId.IPAddress=IPAddress;
                mdDataId.DateTime = mesuringDataTime;
                mdDataId.DataID= dataId;
            }
        }

        public MDDataId GetDataId()
        {
            return mdDataId;
        }

        public string GetDataIdToLog()
        {
            if (mdDataId == null)
            {
                return mdDataId.ToString();
            }
            else return string.Empty;
        }

        public void IncrementDataId()
        {
            mdDataId.IncrementDataId();
        }

        public bool IsFileExsist()
        {
            if (File.Exists(FullPathFileName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void WriteData(string measuredCPUDataToStore)
        {
            Log.Information("PeriodicallyStoreSystem  -> Write to data: {FullPathFileName}", FullPathFileName);
            measuringDataStore.WriteData(measuredCPUDataToStore);
        }

        private bool IsTheMesureTimeStampExpired(DateTime actualDateTime)
        {
            if (!storedFileId.IsTheMesureTimeStampGood(actualDateTime))
                return true;
            else
                return false;
        }

        public void DetermineTheStoreFile(MesuredCPUUsage mesuredCPUUsage)
        {
            if (!storedFileId.IsTheMesureTimeStampGood(mesuredCPUUsage.MeasureTime))
            {
                // Mivel az aktuális adat ideje már lejárt és nem ebben a fájlban tároljuk
                Log.Information("PeriodicallyStoreSystem  -> The {time} stamp is expired.", mesuredCPUUsage.MeasureTime);
                // Zárjuk az aktális fájlt.
                measuringDataStore.Close();
                Log.Information("PeriodicallyStoreSystem  -> {FullPathFileName} is closed.", FullPathFileName);
                measuringDataStore.ChangeFileExtension();
                Log.Information("PeriodicallyStoreSystem  -> {File} is closed and new extenstion is bak.", measuringDataStore.FileName);

                // A dataId-t visszaállítjuk
                mdDataId.DataID = 1;
                // Meghatározzuk az új fájl nevét és fájl írót
                storedFileId.SetActulMeasureFileTimeStamp(mesuredCPUUsage.MeasureTime);
                // A meghatározott új fájlnevet tároljuk
                measuringDataStore.FileName = storedFileId.GetMeasruringPeriodicFileName;

                Log.Information("PeriodicallyStoreSystem -> New File id: {StoreFileID}", storedFileId.GetMeasruringPeriodicFileName);
            }
            else
            {
                Log.Information("PeriodicallyStoreSystem  -> The {time} stamp is good.", mesuredCPUUsage.MeasureTime);
            }
        }

        public ulong GetLastLineId()
        {
            string lastLine = UTF8FileUtilities.ReadLastLine(FullPathFileName);
            if (lastLine != null && lastLine.Length > 0)
            {
                string[] datas = lastLine.Split(";");
                if (datas.Count() >= 3)
                {
                    try
                    {
                        long result = Int64.Parse(datas[3]);
                    }
                    catch 
                    {
                        return 1;
                    }
                }
            }
            return 1;
        }

        public override string ToString()
        {
            return FullPathFileName;
        }
    }



    // https://stackoverflow.com/questions/11625595/read-last-line-of-text-file
    /// Utility class to read last line from a utf-8 text file in a performance sensitive way. The code does not handle a case where more than one line is written at once.
    /// </summary>
    public static class UTF8FileUtilities
    {
        /// <summary>
        /// Read the last line from the file. This method assumes that each write to the file will be terminated with a new line char ('\n')
        /// </summary>
        /// <param name="path">Path of the file to read</param>
        /// <returns>The last line or null if a line could not be read (empty file or partial line write in progress)</returns>
        /// <exception cref="Exception">Opening or reading from file fails</exception>
        public static string ReadLastLine(string path)
        {
            // open read only, we don't want any chance of writing data
            using (System.IO.Stream fs = System.IO.File.OpenRead(path))
            {
                // check for empty file
                if (fs.Length == 0)
                {
                    return null;
                }

                // start at end of file
                fs.Position = fs.Length - 1;

                // the file must end with a '\n' char, if not a partial line write is in progress
                int byteFromFile = fs.ReadByte();
                if (byteFromFile != '\n')
                {
                    // partial line write in progress, do not return the line yet
                    return null;
                }

                // move back to the new line byte - the loop will decrement position again to get to the byte before it
                fs.Position--;

                // while we have not yet reached start of file, read bytes backwards until '\n' byte is hit
                while (fs.Position > 0)
                {
                    fs.Position--;
                    byteFromFile = fs.ReadByte();
                    if (byteFromFile < 0)
                    {
                        // the only way this should happen is if someone truncates the file out from underneath us while we are reading backwards
                        throw new System.IO.IOException("Error reading from file at " + path);
                    }
                    else if (byteFromFile == '\n')
                    {
                        // we found the new line, break out, fs.Position is one after the '\n' char
                        break;
                    }
                    fs.Position--;
                }

                // fs.Position will be right after the '\n' char or position 0 if no '\n' char
                byte[] bytes = new System.IO.BinaryReader(fs).ReadBytes((int)(fs.Length - fs.Position));
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
        }
    }
}

