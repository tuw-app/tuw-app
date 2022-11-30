using DataModel.EFDataModel;
using Microsoft.EntityFrameworkCore;

namespace MeasuringServer.Model
{
    public class MDContext : DbContext
    {
        // https://procodeguide.com/programming/entity-framework-core-in-asp-net-core/
       
        public DbSet<EFCPUUsage> CPUUsage { get; set; }
        public DbSet<EFMeasureDevice> MeasureDevices { get; set; }  

        public MDContext(DbContextOptions<MDContext> options)
                : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EFCPUUsage>().HasKey(x => new { x.IPAddress, x.MeasureTime, x.DataID });

            base.OnModelCreating(modelBuilder);
        }
    }
}
