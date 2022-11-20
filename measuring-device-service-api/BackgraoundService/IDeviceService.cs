using MeasureDeviceProject.Model;

namespace MeasureDeviceServiceAPIProject.BackgraoundService
{
    public interface IDeviceService
    {
        public void Start();
        public void Working();
        public void Stop();

        public MDIPAddress IPAddress{ get; set; }

        public void SetIPAddress(string ipAddress);
    }
}
