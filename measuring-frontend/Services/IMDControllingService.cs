
using System.Threading.Tasks;

namespace MeasureFrontend.Services
{
    public interface IMDControllingService
    {
        public Task<bool> ControlDevice(string controlCommand, string IPAddress);
    }
}
