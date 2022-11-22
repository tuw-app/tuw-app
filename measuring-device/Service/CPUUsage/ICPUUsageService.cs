using System.Threading.Tasks;

namespace MeasureDeviceProject.Service.CPUUsage
{
    public interface ICPUUsageService
    {
        public string GetCPUUsage(bool log);
        public Task<string> ReadCPUUsage();
    }
}
