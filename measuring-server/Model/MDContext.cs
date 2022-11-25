using Microsoft.EntityFrameworkCore;

namespace MeasuringServer.Model
{
    public class MDContext : DbContext
    {
        // https://procodeguide.com/programming/entity-framework-core-in-asp-net-core/
        public DbSet<CPUUsageEF> CPUUsage { get; set; }

        MDContext(DbContextOptions options)
                : base(options)
        { }
    }
}
