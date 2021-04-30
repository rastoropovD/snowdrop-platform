using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snowdrop.DAL.Context;
using Snowdrop.DAL.Repositories;

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

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
        
        public static void AddSnowdropContextInMemory(this IServiceCollection services, string dbName)
        {
            services
                .AddDbContext<SnowdropContext>(options =>
                {
                    options.UseInMemoryDatabase(dbName);
                });
            
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}