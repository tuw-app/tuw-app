using DataModel.MDDataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeasureFrontend.Services
{
    public interface IMDStateService
    {
       public Task<MDState> GetMdSatates(string IPAddress);
    }
}
