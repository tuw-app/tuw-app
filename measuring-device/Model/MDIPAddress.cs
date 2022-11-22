namespace MeasureDeviceProject.Model
{
    public class MDIPAddress
    {
        public string IPAddress { get; set; }

        public MDIPAddress(string iPAddress)
        {
            IPAddress = iPAddress;
        }

        public override string ToString()
        {
            return IPAddress;
        }
    }
}
