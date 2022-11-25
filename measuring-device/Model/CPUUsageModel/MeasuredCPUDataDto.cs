using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureDeviceProject.Model.CPUUsageModel
{
    public class MeasuredCPUDataDto
    {
        private MDIPAddress ipAddress;
        public MDIPAddress IPAddress { get { return ipAddress; } set { ipAddress = value; } }
        private string year;
        public string Year { get { return year; } set { year = value; } }
        private string month;
        public string Month { get { return month; } set { month = value; } }
        private string day;
        public string Day { get { return day; } set { day = value; } }
        private string hour;
        public string Hour { get { return hour; } set { hour = value; } }
        private string minute;
        public string Minute { get { return minute; } set { minute = value; } }
        private string second;
        public string Second { get { return second; } set { second = value; } }
        private string cpuUsage;
        public string CPUUsage { get { return cpuUsage; } set { cpuUsage = value; } }

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

        public MeasuredCPUDataDto(MeasuredCPUDataStore measuredCPUDataStore)
        {
            this.ipAddress = measuredCPUDataStore.IPAddress;
            this.year = measuredCPUDataStore.Year;
            this.month = measuredCPUDataStore.Month;
            this.day = measuredCPUDataStore.Day;
            this.hour = measuredCPUDataStore.Hour;
            this.minute = measuredCPUDataStore.Minute;
            this.second = measuredCPUDataStore.Second;
            this.cpuUsage = measuredCPUDataStore.CPUUsage;
        }
    }
}
