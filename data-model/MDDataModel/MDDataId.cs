using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DataModel.MDDataModel
{

    /// <summary>
    /// Egy mérés eredményének összetett id-je
    /// </summary>
    public class MDDataId : IEquatable<MDDataId>
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
                .Append(DateTime.ToString("yyyy-MM-dd hh:mm:ss,fff"))
                .Append(";")
                .Append(DataID);
            return stringBuilder.ToString();
        }

        public bool Equals([AllowNull] MDDataId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (this.IPAddress != other.IPAddress) return false;
            if (this.DateTime != other.DateTime) return false;
            if (this.DataID != other.DataID) return false;
            return true;
        }
    }
}
