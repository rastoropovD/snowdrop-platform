using AutoMapper;
using NSubstitute;
using Snowdrop.BL.Tests.Unit.Services.Projects;
using Snowdrop.DAL.Repositories;
using Snowdrop.Data;
using Snowdrop.Infrastructure.Dto;
using Xunit;

namespace Snowdrop.BL.Tests.Unit.Services
{
    public sealed class ProjectTests
    {
        private readonly IRepository<Project> m_repository = Substitute.For<IRepository<Project>>();
        private readonly IMapper m_mapper = Substitute.For<IMapper>();

        [Fact]
        public async void CreateProject_NewProject_NoExceptionThrown()
        {
            //Arrange
            int ownerId = 1;
            ProjectDto dto = new ("Title", "Some description", ownerId);
            ProjectsService services = new (m_repository, m_mapper);
            
            //Act
            await services.CreateProject(dto);
        }
        
        [Fact]
        public async void RemoveProject_NoExceptionThrown()
        {
            //Arrange
            int projectId = 1;
            ProjectsService service = new (m_repository, m_mapper);
            
            //Act
            await service.RemoveProject(projectId);
        }
    }
}