using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Snowdrop.DAL.Repositories;
using Snowdrop.Data;
using Snowdrop.Data.Entites;

namespace Snowdrop.BL.Tests.Unit.Services.ProjectMembers
{
    public sealed class ProjectMemberService : IProjectMemberService
    {
        private readonly IRepository<Project> m_repository = default;
        private readonly IMapper m_mapper = default;

        public ProjectMemberService(
            IRepository<Project> repository,
            IMapper mapper)
        {
            m_repository = repository;
            m_mapper = mapper;
        }
        
        public async Task RemoveMember(int projectId, int userId)
        {
            Project project = await m_repository.GetSingle(projectId);
            ProjectMember member = project.Team.SingleOrDefault(m => m.UserId == userId);
            project.Team.Remove(member);
            await m_repository.Update(project);
        }
    }
}