using MeasuringServer.Controllers;
using MeasuringServer.Model;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MeasuringServer.Repository 
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        ILogger<RepositoryWrapper> logger = null;
        private MDContext context=null;

        public RepositoryWrapper(ILogger<RepositoryWrapper> logger, MDContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        private CPUUsageRepository cpuDatas=null;

        public CPUUsageRepository CPUDatas
        {
            get
            {
                if (cpuDatas == null)
                {
                    cpuDatas = new CPUUsageRepository(context);
                }
                return cpuDatas;
            }
        }

        private MeasureDeviceRepository measureDeviceRepository; 

        public MeasureDeviceRepository MeasureDevices
        {
            get
            {
                if (measureDeviceRepository==null)
                {
                    measureDeviceRepository= new MeasureDeviceRepository(context);
                }
                return measureDeviceRepository;
            }
        }


        public async Task SaveAsync()
        {            
            await context.SaveChangesAsync();
        }
    }
}
