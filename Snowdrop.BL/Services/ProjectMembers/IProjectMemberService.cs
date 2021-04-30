using System.Threading.Tasks;

namespace Snowdrop.BL.Tests.Unit.Services.ProjectMembers
{
    public interface IProjectMemberService
    {
        Task RemoveMember(int projectId, int userId);
    }
}