using DataModel.EFDataModel;
using MeasuringServer.Model;
using MeasuringServer.Repository.Base;
using System;
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
        
        public EFMeasureDevice Get(string IPAddress)
        {
            return FindByCondition(md => md.Name.CompareTo(IPAddress)==0).FirstOrDefault();
        }

        public EFMeasureDevice GetByIPAddress(string IPAdress)
        {
            return FindByCondition(md => md.Name.CompareTo(IPAdress)==0).FirstOrDefault();
        }

        public IQueryable<EFMeasureDevice> GetAll()
        {
            return FindAll();
        }

        public void Insert(EFMeasureDevice md)
        {
            //int id = FindAll().ToList().Select(md => md.Id).Max() + 1;
            //Console.WriteLine(id);
            //md.Id = id;
            Create(md);
        }


        public void Update(int id, int measuringInterval)
        {
            EFMeasureDevice selecteddevice = Get(id);
            if (selecteddevice != null && selecteddevice.Name.Length != 0)
            {
                selecteddevice.Interval = measuringInterval;
                Update(id, selecteddevice);
            }                
        }

        public void Update(EFMeasureDevice entity)
        {
            Update(entity.Id, entity);
        }

        public bool IsExsist(EFMeasureDevice device)
        {
            EFMeasureDevice selecteddevice = Get(device.Id);
            if (selecteddevice != null && selecteddevice.Name.Length != 0)
            {
                return (selecteddevice.Name == string.Empty);
            }
            else
                return false;
        }

        public int CountOfDevices()
        {
            return FindAll().Count();
        }

        public bool IsExsist(string IPAddress)
        {
            if (CountOfDevices() == 0)
                return false;
            EFMeasureDevice selecteddevice = Get(IPAddress);            
            if (selecteddevice != null && selecteddevice.Name.Length != 0)
            {
                return (selecteddevice.Name.Length>0);
            }
            else
                return false;
        }


    }
}
