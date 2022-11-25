using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureDeviceProject.Model.CPUUsageModel
{
    public class MeasuredCPUDataStore
    {

        private MDDataId dataId;
        private MeasuredCPUUsage measuredCPUUsage;

        private string measuredDataToTextFile;
        public string MeasuredCPUDataToStore { get { return MeasuredDataToTextFile(); } }

        public MDIPAddress IPAddress { get { return dataId.IPAddress; } }
        public string Year { get { return dataId.DateTime.Year.ToString(); } }
        public string Month { get { return dataId.DateTime.Month.ToString(); } }
        public string Day { get { return dataId.DateTime.Day.ToString(); } }
        public string Hour { get { return dataId.DateTime.Hour.ToString(); } }
        public string Minute { get { return dataId.DateTime.Minute.ToString(); } }
        public string Second { get { return dataId.DateTime.Second.ToString(); } }
        public string CPUUsage { get { return measuredCPUUsage.ToStore(); } }



        public MeasuredCPUDataStore(MDDataId Id, MeasuredCPUUsage measuredCPUUsage)
        {
            dataId = Id;
            this.measuredCPUUsage = measuredCPUUsage;
        }

        private string MeasuredDataToTextFile()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(dataId).Append(";").Append(measuredCPUUsage.ToStore());
            return sb.ToString();
        }
    }
}
