using System.Threading.Tasks;
using AutoMapper;
using Snowdrop.DAL.Repositories;
using Snowdrop.Data;
using Snowdrop.Infrastructure.Dto;

namespace Snowdrop.BL.Tests.Unit.Services.Projects
{
    public sealed class ProjectServices : IProjectService
    {
        private readonly IRepository<Project> m_repository = default;
        private readonly IMapper m_mapper = default;

        public ProjectServices(
            IRepository<Project> repository,
            IMapper mapper)
        {
            m_repository = repository;
            m_mapper = mapper;
        }
        
        public async Task CreateProject(ProjectDto dto)
        {
            await m_repository.Insert(m_mapper.Map<Project>(dto));
        }

        public async Task RemoveProject(int projectId)
        {
            await m_repository.Delete(projectId);
        }
    }
}