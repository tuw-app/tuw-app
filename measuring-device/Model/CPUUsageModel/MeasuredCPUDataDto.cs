using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureDeviceProject.Model.CPUUsageModel
{
    public class MeasuredCPUDataDto
    {
        public MDIPAddress IPAddress { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string Hour { get; set; }
        public string Minute { get; set; }
        public string Second { get; set; }
        public string CPUUsage { get; set; }

        public MeasuredCPUDataDto(MDIPAddress iPAddress, string year, string month, string day, string hour, string minute, string second, string cPUUsage)
        {
            IPAddress = iPAddress;
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
            CPUUsage = cPUUsage;
        }
    }
}
