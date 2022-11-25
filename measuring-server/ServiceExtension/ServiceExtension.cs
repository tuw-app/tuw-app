using MeasuringServer.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace MeasuringServer.ServiceExtension
{
    public static class ServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureWrapperRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper,RepositoryWrapper>();
        }
    }
}
