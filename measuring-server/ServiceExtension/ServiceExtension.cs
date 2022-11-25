using MeasuringServer.Model;
using MeasuringServer.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Storage;

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
        public static void ConfigureMysql(this IServiceCollection services, IConfiguration configuration)
        {

            string dbConnectionString = configuration.GetConnectionString("MySql");
            //services.AddDbContext<MDContext>(opt => opt.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString)));
            services.AddDbContext<MDContext>(opt => opt.UseMySql(dbConnectionString));
        }
    }
}
