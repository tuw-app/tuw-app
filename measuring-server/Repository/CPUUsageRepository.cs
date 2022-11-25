using MeasureDeviceProject.Model;
using MeasuringServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeasuringServer.Repository
{
    public class CPUUsageRepository : RepostitoryBase<CPUUsageEF>, ICPUUsageEFRepository
    {
        public CPUUsageRepository(MDContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateCPUUsage(CPUUsageEF cpuUsage)
        {
            Create(cpuUsage);
        }

        public void UpdateCPUUsage(CPUUsageEF cpuUsage)
        {
            Update(cpuUsage);
        }

        public void DeleteCPUUsage(CPUUsageEF cpuUsage)
        {
            Update(cpuUsage);
        }

        public IEnumerable<CPUUsageEF> GetAllCPUUsageAsync()
        {
            return FindAll()
                        .OrderBy(u => u.IPAddress).ThenBy(u => u.MeasureTime).ThenBy(u => u.DataID)
                        .ToList();
        }

        public CPUUsageEF GetCPUUsageByIdAsync(MDDataId other)
        {
            MDDataId otherID = other.Get;
            return FindByCondition(cpuUsage => cpuUsage.GetId().Equals(other.Get))
                        .FirstOrDefault();
        }


    }
}
