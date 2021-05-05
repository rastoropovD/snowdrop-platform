using System.Collections.Generic;
using System.Threading.Tasks;
using Snowdrop.Infrastructure.Dto;

namespace Snowdrop.BL.Tests.Unit.Services.ProjectMembers
{
    public interface IProjectMemberService
    {
        Task AddMember(ProjectMemberDto dto);
        Task RemoveMember(int projectId, int userId);
        Task<IEnumerable<ProjectMemberDto>> GetAllMembers(int projectId);
    }
}