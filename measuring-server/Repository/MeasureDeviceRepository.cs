using MeasuringServer.Model;
using MeasuringServer.Repository.Base;
using System.Linq;

namespace MeasuringServer.Repository
{
    public class MeasureDeviceRepository : RepositoryBase<EFMeasureDevice>, IMeasureDeviceRepository
    {

        public MeasureDeviceRepository(MDContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void Delete(int id)
        {
            EFMeasureDevice md = Get(id);
            if (md != null || md.Name!=string.Empty)
            {
                Delete(md);
            }
        }

        public EFMeasureDevice Get(long id)
        {
            return FindByCondition(md=>md.Id== id).FirstOrDefault();
        }

        public IQueryable<EFMeasureDevice> GetAll()
        {
            return FindAll();
        }

        public void Insert(EFMeasureDevice md)
        {
            Create(md);
        }

        public bool IsExsist(EFMeasureDevice device)
        {
            EFMeasureDevice selecteddevice = Get(device.Id);
            if (selecteddevice != null)
            {
                return (selecteddevice.Name == string.Empty);
            }
            else
                return false;
        }
    }
}
