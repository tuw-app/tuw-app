using DataModel.MDDataModel;
using System.Threading.Tasks;

namespace MeasureFrontend.Services
{
    public interface IMDIntervalService
    {
        public Task<long> GetMDInterval(string name);
    }
}
