using System;
using System.Collections.Generic;
using System.Linq;

using DataModel.EFDataModel;
using DataModel.MDDataModel;
using MeasuringServer.Model;
using MeasuringServer.Model.Paging;
using MeasuringServer.Repository.Base;


namespace MeasuringServer.Repository
{
    // https://medium.com/c-sharp-progarmming/quick-start-asp-net-core-3-1-entity-framework-core-cqrs-react-js-series-c8a427385aed
    // https://code-maze.com/net-core-web-development-part4/
    public class CPUUsageRepository : RepositoryBase<EFCPUUsage>, ICPUUsageEFRepository
    {
        public CPUUsageRepository(MDContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateCPUUsage(EFCPUUsage cpuUsage)
        {
            try {

                Create(cpuUsage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void UpdateCPUUsage(EFCPUUsage cpuUsage)
        {
        }

        public void DeleteCPUUsage(EFCPUUsage cpuUsage)
        {
        }

        public List<EFCPUUsage> GetAllCPUUsage()
        {
            return FindAll()
                        .OrderBy(u => u.IPAddress).ThenBy(u => u.MeasureTime).ThenBy(u => u.DataID)
                        .ToList();
        }

        public EFCPUUsage GetCPUUsageById(MDDataId id)
        {
            try
            {
                Console.WriteLine($"Serched id:{id}");
                var all = FindAll();
                foreach (EFCPUUsage usage in all)
                {
                    MDDataId otherID = usage.GetId();
                    if (otherID.Equals(id))
                        return usage;
                }
                return new EFCPUUsage();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new EFCPUUsage();
            }

            //return FindByCondition(cpuUsage => cpuUsage.GetId().Equals(id))
            //            .FirstOrDefault();
        }

        public bool IsExsist(MDDataId CPUUsageID)
        {
            try
            {
                EFCPUUsage data = GetCPUUsageById(CPUUsageID);
                if (data.IdIsOk())
                    return false;
                else return true;
            }
            
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public PagedList<EFCPUUsage> GetAllCPUUsageOfSpecificDevicePaged(string IPAddress, int page, int pagesize)
        {
            try
            {
                PagedList<EFCPUUsage> result = new PagedList<EFCPUUsage>();
                result.List = FindAll()
                    .Where(cpuUsage => cpuUsage.IPAddress == IPAddress)
                    .Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .OrderBy(cpuusage => cpuusage.IPAddress)
                    .ThenBy(cpuusage => cpuusage.MeasureTime)
                    .ThenBy(cpuusage => cpuusage.DataID)
                    .ToList();
                result.SetPageData(page, pagesize, FindAll().Where(cpuUsage => cpuUsage.IPAddress == IPAddress).ToList().Count);
                return result;
            }                        
            catch(Exception e)
            {

                Console.WriteLine(e.Message);
                return new PagedList<EFCPUUsage>();
            }

        }
    }
}
