using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureDeviceProject.Model
{

    /// <summary>
    /// Egy mérés eredményének összetett id-je
    /// </summary>
    public class MDDataId
    {
        public MDIPAddress IPAddress { get; set; }
        public DateTime DateTime { get; set; }       
        public ulong DataID { get; set; }

        public MDDataId(MDIPAddress ipAddress, DateTime dateTime, ulong dataID = 1)
        {
            IPAddress = ipAddress;
            DateTime = dateTime;
            DataID = dataID;
        }

        public void IncrementDataId()
        {
            DataID = DataID + 1;
        }

        public string GetId { get { return ToString(); } }

        public override string ToString()
        {
            StringBuilder stringBuilder= new StringBuilder();
            stringBuilder.Append(IPAddress.ToString())
                .Append(";")
                .Append(DateTime.ToString("yyyy-MM-dd;hh-mm-ss-ffff"))
                .Append(";")
                .Append(DataID);
            return stringBuilder.ToString();
        }
    }
}
