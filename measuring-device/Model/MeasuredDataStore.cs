using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureDeviceProject.Model
{
    public class MeasuredDataStore
    {
        private string measuredData;
        public string MeasuredData { get {return MeasuredData;} }

        public MeasuredDataStore(string Id, string data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Id).Append(";").Append(data);
            measuredData=sb.ToString();
        }
    }
}
