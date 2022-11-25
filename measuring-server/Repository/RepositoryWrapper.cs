using MeasuringServer.Model;
using System.Threading.Tasks;

namespace MeasuringServer.Repository 
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private MDContext context=null;

        public RepositoryWrapper(MDContext context)
        {
            this.context = context;
        }

        private CPUUsageRepository cpuDatas=null;

        public CPUUsageRepository CPUDatas
        {
            get
            {
                if (CPUDatas==null)
                {
                    cpuDatas = new CPUUsageRepository(context);
                }
                return CPUDatas;
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
