using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureDeviceProject.Service.FileWriter
{
    public interface IMeasuringDataStore
    {
        public void WriteData(string data);
        public void Close();
    }
}
