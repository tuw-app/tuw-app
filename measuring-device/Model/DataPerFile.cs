using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureDeviceProject.Model
{
    public class DataPerFile
    {
        private Dictionary<string, ulong> dataPerFile = null;

        public DataPerFile() 
        { 
            dataPerFile= new Dictionary<string, ulong>();
        }

        public void Add(string fileName,ulong dataPerFileName)
        {
            dataPerFile.Add(fileName, dataPerFileName);
        }

        public void Remove(string fileName)
        {
            dataPerFile.Remove(fileName);
        }

        public ulong Get(string fileName) 
        { 
            if (dataPerFile == null)
                return 0;
            else if (dataPerFile.ContainsKey(fileName))
                return dataPerFile[fileName]; 
            return 0;
        }
    }
}
