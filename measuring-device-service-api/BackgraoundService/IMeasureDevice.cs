using MeasureDeviceProject.Model;

namespace MeasureDeviceServiceAPIProject.BackgraoundService
{
    public interface IMeasureDevice
    {
        public MDIPAddress IPAddress { get; set; }

        public void StartDevice();
        public void StopDevice();
    }
}
