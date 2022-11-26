using MeasureDeviceProject.Model;
using MeasuringServer.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeasuringServer.Repository
{
    // https://medium.com/c-sharp-progarmming/quick-start-asp-net-core-3-1-entity-framework-core-cqrs-react-js-series-c8a427385aed
    // https://code-maze.com/net-core-web-development-part4/
    public class CPUUsageRepository : RepostitoryBase<CPUUsageEF>, ICPUUsageEFRepository
    {
        public CPUUsageRepository(MDContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateCPUUsage(CPUUsageEF cpuUsage)
        {
            try { 


                Console.WriteLine("CreateCPUUsage");
                Create(cpuUsage);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void UpdateCPUUsage(CPUUsageEF cpuUsage)
        {
            Update(cpuUsage);
        }

        public void DeleteCPUUsage(CPUUsageEF cpuUsage)
        {
            Update(cpuUsage);
        }

        public List<CPUUsageEF> GetAllCPUUsage()
        {
            return FindAll()
                        .OrderBy(u => u.IPAddress).ThenBy(u => u.MeasureTime).ThenBy(u => u.DataID)
                        .ToList();
        }

        public CPUUsageEF GetCPUUsageById(MDDataId id)
        {
            Console.WriteLine($"Serched id:{id}");
            var all = FindAll();
            foreach(CPUUsageEF usage in all)
            {
                MDDataId otherID = usage.GetId();
                if (otherID.Equals(id))
                    return usage;
            }
            return new CPUUsageEF();

            //return FindByCondition(cpuUsage => cpuUsage.GetId().Equals(id))
            //            .FirstOrDefault();
        }

        public bool IsExsist(MDDataId CPUUsageID)
        {
            CPUUsageEF data=GetCPUUsageById(CPUUsageID);
            if (data.IdIsOk())
                return false;
            else return true;
        }

        public List<CPUUsageEF> GetAllCPUUsageOfSpecificDevicePaged(string iPAddress, int page, int pagesize)
        {
            return FindAll()
                .Skip((page-1)*pagesize)
                .Take(pagesize)
                .OrderBy(cpuusage => cpuusage.IPAddress)
                .ThenBy(cpuusage =>cpuusage.MeasureTime)
                .ThenBy(cpuusage =>cpuusage.DataID)
                .ToList();
        }
    }
}
