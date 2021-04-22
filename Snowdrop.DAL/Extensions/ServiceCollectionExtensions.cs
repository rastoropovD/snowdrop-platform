using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snowdrop.DAL.Context;

namespace Snowdrop.DAL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSnowdropContext(this IServiceCollection services, string connectionString)
        {
            services
                .AddDbContext<SnowdropContext>(options =>
                {
                    options
                        .UseLazyLoadingProxies()
                        .UseSqlServer(connectionString, builder => builder.MigrationsAssembly("Snowdrop.DAL"));
                });

            services.AddScoped<SnowdropContext>();
        }
        
    }
}