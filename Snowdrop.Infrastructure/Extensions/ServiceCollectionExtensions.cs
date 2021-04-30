using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Snowdrop.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSnowdropMapper(this IServiceCollection services)
        {
            services
                .AddAutoMapper(expression =>
                    expression.AddMaps(Assembly.GetAssembly(typeof(ServiceCollectionExtensions))));
        }
    }
}