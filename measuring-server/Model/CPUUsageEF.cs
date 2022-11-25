using MeasureDeviceProject.Model.CPUUsageModel;
using MeasureDeviceProject.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System;

using MeasuringServer.Static;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MeasuringServer.Model
{
    [Table("cpuusage")]
    public class CPUUsageEF : IEquatable<CPUUsageEF>
    {
        [Required(ErrorMessage = "Ip address is required")]
        private string ipAddress;
        public string IPAddress { get { return ipAddress; } set { ipAddress = value; } }

        [Required(ErrorMessage = "Measure time is required")]
        private DateTime measureTime;
        public DateTime MeasureTime { get { return measureTime; } set { measureTime = value; } }

        private ulong dataID;
        public ulong DataID { get => dataID; set => dataID = value; }

        [Required(ErrorMessage = "CPU usage is required")]
        private double cpuUsage;
        public double CPUUsage { get => CPUUsage; set => CPUUsage = value; }


        private void NullData()
        {
            ipAddress = string.Empty;
            measureTime = DateTime.MinValue;
            CPUUsage = double.MinValue;
            dataID = ulong.MinValue;
        }

        public CPUUsageEF() 
        {
            NullData();
        }

        public bool DataIsOk()
        {
            if (!string.IsNullOrEmpty(IPAddress))
                return true;
            else
                return false;
        }

        public MDDataId GetId()
        {
            MDDataId id = new MDDataId(new MDIPAddress(IPAddress), measureTime, dataID);
            return id;
        }

        public bool Equals([AllowNull] CPUUsageEF other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (this.IPAddress!= other.IPAddress) return false;
            if (this.dataID != other.dataID) return false;
            if (this.measureTime!= other.measureTime) return false;
            return true;
        }

        public CPUUsageEF(MDSended dataFromMDSystem)
        {
            if (string.IsNullOrEmpty(dataFromMDSystem.ToString()))
            {
                NullData();
            }
            else
            {
                string[] data = dataFromMDSystem.ToString().Split(";");
                if (data.Length== 3)
                {
                    try
                    {
                        ipAddress = data[0];
                        measureTime = data[1].ToDateTime();
                        dataID = long.Parse(data[2]);
                        CPUUsage = double.Parse(data[3].Replace(",", "."));
                    }
                    catch { NullData(); }
                    if (string.IsNullOrEmpty(IPAddress))
                        NullData();

                }
                else
                {
                    NullData();
                }
            }
        }
    }
}
