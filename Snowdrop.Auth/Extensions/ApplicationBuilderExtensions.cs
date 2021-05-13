using Microsoft.AspNetCore.Builder;

namespace Snowdrop.Auth.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void AddAuth(this IApplicationBuilder builder)
        {
            builder.UseAuthentication();
            builder.UseAuthorization();
        }
    }
}