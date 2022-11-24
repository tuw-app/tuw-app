using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureDeviceProject.Service.FileWriter
{
    public class MeasuringDataStoreException : Exception
    {
        public MeasuringDataStoreException(string message) : base(message)
        {
        }
    }
}
