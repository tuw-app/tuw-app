using MeasureDeviceProject.Model.MeasureElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureDeviceProject.Model
{
    public class MeasuredCPUDataStore
    {

        private MDDataId Id;
        private MeasuredCPUUsage MeasuredCPUUsage;
        private string measuredDataToTextFile;

        public MDIPAddress IPAddress { get { return Id.IPAddress; } }
        public string Year { get { return Id.DateTime.Year.ToString(); } }
        public string Month { get { return Id.DateTime.Month.ToString(); } }
        public string Day { get { return Id.DateTime.Day.ToString(); } }
        public string Hour { get { return Id.DateTime.Hour.ToString(); } }
        public string Minute { get { return Id.DateTime.Minute.ToString(); } }
        public string Second { get { return Id.DateTime.Second.ToString(); } }

        public string CPUUsage { get { return MeasuredCPUUsage.ToStore()} }

        public string MeasuredCPUDataToStore { get {return measuredDataToTextFile; } }

        public MeasuredCPUDataStore(MDDataId Id, MeasuredCPUUsage measuredCPUUsage)
        {
            this.Id = Id;
            this.MeasuredCPUUsage= measuredCPUUsage;

            StringBuilder sb = new StringBuilder();
            sb.Append(Id).Append(";").Append(measuredCPUUsage.ToStore());
            measuredDataToTextFile=sb.ToString();
        }
    }
}
