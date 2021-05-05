using System.Collections.Generic;
using AutoMapper;
using NSubstitute;
using Snowdrop.BL.Tests.Unit.Services.ProjectMembers;
using Snowdrop.DAL.Repositories;
using Snowdrop.Data;
using Snowdrop.Data.Entites;
using Snowdrop.Data.Enum;
using Snowdrop.Infrastructure.Dto;
using Xunit;

namespace Snowdrop.BL.Tests.Unit.Services
{
    public sealed class ProjectMemberTests
    {
        private readonly IRepository<Project> m_projectRepository = Substitute.For<IRepository<Project>>();
        private readonly IMapper m_mapper = Substitute.For<IMapper>();
        private IProjectMemberService m_service = default;

        [Fact]
        public async void AddMember_NoExceptionThrown()
        {
            //Arrange
            int projectId = 1;
            int userId = 1;
            ProjectMemberDto dto = new (userId, projectId, TeamMemberRole.Developer);
            Project mockProject = new ()
            {
                Id = projectId,
                Team = new List<ProjectMember>()
            };
            m_projectRepository.GetSingle(projectId).Returns(mockProject);
            m_service = new ProjectMemberService(m_projectRepository, m_mapper);

            //Act
            await m_service.AddMember(dto);
        }

        
        [Fact]
        public async void RemoveMember_MemberExists_NoExceptionThrown()
        {
            //Arrange
            int projectId = 1;
            int userId = 1;
            Project mockProject = new()
            {
                Id = projectId,
                Team = new List<ProjectMember>()
                {
                    new() {ProjectId = projectId, UserId = userId}
                }
            };
            m_projectRepository.GetSingle(projectId).Returns(mockProject);
            m_service = new ProjectMemberService(m_projectRepository, m_mapper);

            //Act
            await m_service.RemoveMember(projectId, userId);
        }

        
        [Fact]
        public async void GetAllMembers_NoExceptionThrown()
        {
            //Arrange
            int projectId = 1;
            int userId = 1;
            Project mockProject = new()
            {
                Id = projectId,
                Team = new List<ProjectMember>()
                {
                    new() {ProjectId = projectId, UserId = userId}
                }
            };
            m_projectRepository.GetSingle(projectId).Returns(mockProject);
            m_service = new ProjectMemberService(m_projectRepository, m_mapper);
            
            //Act
            IEnumerable<ProjectMemberDto> members = await m_service.GetAllMembers(projectId);
            
            //Assert
            Assert.NotEmpty(members);
        }
    }
}