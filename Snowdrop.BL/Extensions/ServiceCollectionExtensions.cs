using Microsoft.Extensions.DependencyInjection;
using Snowdrop.BL.Tests.Unit.Services.ProjectMembers;
using Snowdrop.BL.Tests.Unit.Services.Projects;
using Snowdrop.BL.Tests.Unit.Services.Users;

namespace Snowdrop.BL.Tests.Unit.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSnowdropServices(this IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectsService>();
            services.AddScoped<IProjectMemberService, ProjectMemberService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}