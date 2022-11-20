using System.Threading.Tasks;

namespace MeasureDeviceProject.Model.MeasureElements
{
    public interface ITempUsageSubDevice
    {
        public string GetCurrentTemperature();
        public Task<string> GetCurrentCpuUsage();
    }
}
