using System.Collections.Generic;
using AutoMapper;
using NSubstitute;
using Snowdrop.BL.Tests.Unit.Services.ProjectMembers;
using Snowdrop.DAL.Repositories;
using Snowdrop.Data;
using Snowdrop.Data.Entites;
using Xunit;

namespace Snowdrop.BL.Tests.Unit.Services
{
    public sealed class ProjectMemberTests
    {
        private ProjectMemberService m_service = default;
        private IRepository<Project> m_repository = default;
        private readonly IMapper m_mapper = Substitute.For<IMapper>();

        [Fact]
        public async void RemoveMember_ProjectExists_NoExceptionThrown()
        {
            //Arrange
            int projectId = 1;
            int userId = 1;
            Project mockProject = new Project()
            {
                Id = projectId,
                Team = new List<ProjectMember>() {new ProjectMember {ProjectId = projectId, UserId = userId}}
            };
            m_repository = Substitute.For<IRepository<Project>>();
            m_repository.GetSingle(projectId).Returns(mockProject);
            m_service = new ProjectMemberService(m_repository, m_mapper);

            //Act
            await m_service.RemoveMember(projectId, userId);
        }
        
    }
}