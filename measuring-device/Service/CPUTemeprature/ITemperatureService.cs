namespace MeasureDeviceProject.Service.CPUTemeprature
{
    public interface ITemperatureService
    {
        public string GetTemperature(bool log);
        public void ReadTemperature();
    }
}
