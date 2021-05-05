using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Snowdrop.DAL.Repositories;
using Snowdrop.Data;
using Snowdrop.Data.Entites;
using Snowdrop.Infrastructure.Dto;

namespace Snowdrop.BL.Tests.Unit.Services.ProjectMembers
{
    public sealed class ProjectMemberService : IProjectMemberService
    {
        private readonly IRepository<Project> m_projectRepository = default;
        private readonly IMapper m_mapper = default;
        
        public ProjectMemberService(
            IRepository<Project> projectRepository,
            IMapper mapper)
        {
            m_projectRepository = projectRepository;
            m_mapper = mapper;
        }
        
        public async Task AddMember(ProjectMemberDto dto)
        {
            Project project = await m_projectRepository.GetSingle(dto.ProjectId);
            ProjectMember member = m_mapper.Map<ProjectMember>(dto);
            project.Team.Add(member);
            await m_projectRepository.Update(project);
        }

        public async Task RemoveMember(int projectId, int userId)
        {
            Project project = await m_projectRepository.GetSingle(projectId);
            ProjectMember member = project.Team.SingleOrDefault(m => m.UserId == userId);
            project.Team.Remove(member);
            await m_projectRepository.Update(project);
        }

        public async Task<IEnumerable<ProjectMemberDto>> GetAllMembers(int projectId)
        {
            Project project = await m_projectRepository.GetSingle(projectId);
            return project.Team.Select(m => m_mapper.Map<ProjectMemberDto>(m));
        }
    }
}