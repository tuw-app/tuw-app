namespace MeasureDeviceProject.Service.CPUTemeprature
{
    public class CPUTemperatureService : ITemperatureService
    {
        private OHMTemperatureService temperatureService=new OHMTemperatureService();

        public string GetTemperature(bool log)
        {
            return temperatureService.GetTemperature(log);
        }

        public void ReadTemperature()
        {
            temperatureService.ReadTemperature();
        }
    }
}
