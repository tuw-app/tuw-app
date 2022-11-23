using System.Threading.Tasks;

namespace MeasureDeviceProject.Service.CPUUsage
{
    public interface ICPUUsageService
    {
        public string GetCPUUsage(bool log=false);
        public string GetCPUUsageToLog();
        public Task<string> ReadCPUUsage();
    }
}
