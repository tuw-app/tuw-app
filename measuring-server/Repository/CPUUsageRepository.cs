using MeasureDeviceProject.Model;
using MeasuringServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeasuringServer.Repository
{
    // https://medium.com/c-sharp-progarmming/quick-start-asp-net-core-3-1-ef-core-cqrs-react-js-series-part-4-repository-pattern-748faaa715f1
    // https://code-maze.com/net-core-web-development-part4/
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
