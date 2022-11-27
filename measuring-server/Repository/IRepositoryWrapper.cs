using System.Threading.Tasks;

namespace MeasuringServer.Repository
{
    public interface IRepositoryWrapper
    {
        public CPUUsageRepository CPUDatas { get;}
        public MeasureDeviceRepository MeasureDevices { get;}
        public Task SaveAsync();
    }
}
