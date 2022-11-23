using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureDeviceProject.Model
{
    public class MDDataId
    {
        public MDIPAddress IPAddress { get; set; }
        public ulong MeasuringId { get; set; }
        public ulong DataID { get; set; }

        public MDDataId(MDIPAddress iPAddress, ulong measuringId=1, ulong dataID = 1)
        {
            IPAddress = iPAddress;
            MeasuringId = measuringId;
            DataID = dataID;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder= new StringBuilder();
            stringBuilder.Append(IPAddress.ToString())
                .Append(":")
                .Append(MeasuringId)
                .Append(":")
                .Append(DataID);
            return stringBuilder.ToString();
        }
    }
}
