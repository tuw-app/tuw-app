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
    }
}
