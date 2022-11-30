using DataModel.EFDataModel;
using MeasuringServer.Repository.Base;

namespace MeasuringServer.Repository
{
    public interface IMeasureDeviceRepository : IRepositoryBase<EFMeasureDevice>
    {
        EFMeasureDevice Get(string IPAddress);
        public bool IsExsist(EFMeasureDevice device);
        public bool IsExsist(string IPAddress);
        public EFMeasureDevice GetByIPAddress(string ipAddress);
        public void Update(int id, long measuringInterval);
    }
}
