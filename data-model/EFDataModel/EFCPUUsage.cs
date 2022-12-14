using System.ComponentModel.DataAnnotations.Schema;
using System;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DataModel.MDDataModel;
using DataModel.Static;

namespace DataModel.EFDataModel
{
    // https://stackoverflow.com/questions/3633262/convert-datetime-for-mysql-using-c-sharp
    [Table("cpuusage")]
    public class EFCPUUsage : IEquatable<EFCPUUsage>
    {
        private string ipAddress;
        private DateTime measureTime;
        private ulong dataID;
        private double cpuUsage;

        [Column("ipaddress")]
        [Required(ErrorMessage = "Ip address is required")]
        public string IPAddress { get { return ipAddress; } set { ipAddress = value; } }

        [Column("measuretime")]
        [Required(ErrorMessage = "Measure time is required")]
        public DateTime MeasureTime { get { return measureTime; } set { measureTime = value; } }

        [Column("dataid")]
        [Required(ErrorMessage = "Data id is required")]
        public ulong DataID { get => dataID; set => dataID = value; }


        [Column("cpuusage")]
        [Required(ErrorMessage = "CPU usage is required")]
        public double CPUUsage { get => cpuUsage; set => cpuUsage = value; }


        private void NullData()
        {
            ipAddress = string.Empty;
            measureTime = DateTime.MinValue;
            CPUUsage = double.MinValue;
            dataID = ulong.MinValue;
        }

        public EFCPUUsage()
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

        public bool IdIsOk()
        {
            return DataIsOk();
        }

        public MDDataId GetId()
        {
            MDDataId id = new MDDataId(new MDIPAddress(IPAddress), measureTime, dataID);
            return id;
        }

        public bool Equals([AllowNull] EFCPUUsage other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (IPAddress != other.IPAddress) return false;
            if (dataID != other.dataID) return false;
            if (measureTime != other.measureTime) return false;
            return true;
        }

        public EFCPUUsage(MDSendedDataFromDeviceToServer dataFromMDSystem)
        {
            if (string.IsNullOrEmpty(dataFromMDSystem.ToString()))
            {
                NullData();
            }
            else
            {
                string[] data = dataFromMDSystem.ToString().Split(";");
                if (data.Length == 4)
                {
                    try
                    {
                        ipAddress = data[0];
                        measureTime = data[1].ToDateTime();
                        dataID = ulong.Parse(data[2]);
                        CPUUsage = double.Parse(data[3]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        NullData();

                    }
                    if (string.IsNullOrEmpty(IPAddress))
                        NullData();

                }
                else
                {
                    NullData();
                }
            }
        }

        public override string ToString()
        {

            return $"{IPAddress} - {MeasureTime.ToString("yyyy.MM.dd HH:mm:ss,ffff")} : {dataID} : {CPUUsage}";
        }
    }
}
