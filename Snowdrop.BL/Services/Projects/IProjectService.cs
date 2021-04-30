using System.Threading.Tasks;
using Snowdrop.Infrastructure.Dto;

namespace Snowdrop.BL.Tests.Unit.Services.Projects
{
    public interface IProjectService
    {
        Task CreateProject(ProjectDto dto);
        Task RemoveProject(int projectId);
    }
}