using MeasureDeviceProject.Model.MeasureElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureDeviceProject.Model
{
    public class MeasuredCPUDataStore
    {
        private string measuredData;
        public string MeasuredCPUDataToStore { get {return measuredData; } }

        public MeasuredCPUDataStore(MDDataId Id, MesuredCPUUsage data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Id).Append(";").Append(data);
            measuredData=sb.ToString();
        }
    }
}
