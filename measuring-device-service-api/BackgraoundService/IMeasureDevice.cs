using MeasureDeviceProject.Model;

namespace MeasureDeviceServiceAPIProject.BackgraoundService
{
    public interface IMeasureDevice
    {
        public MDIPAddress IPAddress { get; set; }

        public void Stop();
        public void Start();
        public void StopMeasuring();
        public void StartMeasuring();

    }

    public interface IMeasureDevice10 
    {
        public MDIPAddress IPAddress { get; set; }

        public void Stop();
        public void Start();
        public void StopMeasuring();
        public void StartMeasuring();
    }

    public interface IMeasureDevice20 
    {
        public MDIPAddress IPAddress { get; set; }

        public void Stop();
        public void Start();
        public void StopMeasuring();
        public void StartMeasuring();
    }

    public interface IMeasureDevice30 
    {
        public MDIPAddress IPAddress { get; set; }

        public void Stop();
        public void Start();
        public void StopMeasuring();
        public void StartMeasuring();
    }
}
