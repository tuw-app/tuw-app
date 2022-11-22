namespace MeasureDeviceServiceAPIProject.Service
{
    public interface IMeasureSendingDataService
    {
        public void Start();
        public void Stop();
        public void SetMeasuringInterval(int interval);
        public double GetMeasuringInterval();
    }
}
