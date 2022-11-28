using MeasuringServer.Model;
using MeasuringServer.Repository.Base;

namespace MeasuringServer.Repository
{
    public interface IMeasureDeviceRepository : IRepositoryBase<EFMeasureDevice>
    {
        public bool IsExsist(EFMeasureDevice device);
        public EFMeasureDevice GetByIPAddress(string ipAddress);
    }
}
