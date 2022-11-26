using System.Threading.Tasks;

namespace MeasuringServer.Repository
{
    public interface IRepositoryWrapper
    {
        public CPUUsageRepository CPUDatas { get;}
        public Task SaveAsync();
    }
}
