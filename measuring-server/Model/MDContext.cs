using Microsoft.EntityFrameworkCore;

namespace MeasuringServer.Model
{
    public class MDContext : DbContext
    {
        // https://procodeguide.com/programming/entity-framework-core-in-asp-net-core/
        public DbSet<CPUUsageEF> CPUUsage { get; set; }

        public MDContext(DbContextOptions<MDContext> options)
                : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CPUUsageEF>().HasKey(x => new { x.IPAddress, x.MeasureTime, x.DataID });

            base.OnModelCreating(modelBuilder);
        }
    }
}
