using MeasureDeviceProject.Model;

namespace MeasureDeviceServiceAPIProject.BackgraoundService
{
    public interface IMeasureDevice
    {
        public MDIPAddress IPAddress { get; set; }

        public void Start();
        public void Stop();
    }
}
