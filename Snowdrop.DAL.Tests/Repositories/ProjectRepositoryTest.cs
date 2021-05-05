using System.Threading.Tasks;
using Snowdrop.Data;
using Xunit;

namespace Snowdrop.DAL.Tests.Repositories
{
    public sealed class ProjectRepositoryTest : BaseRepositoriesTest<Project>
    {
        [Fact]
        public async Task Insert_Project_ProjectInserted()
        {
            //Arrange
            int projectId = 1;
            Project project = new()
            {
                Id = projectId,
                OwnerId = 1,
                Title = "TestProject",
                Description = "Some description"
            };

            //Act
            await Repository.Insert(project);
            Project selectedProject = await Repository.GetSingle(projectId);

            //Assert
            Assert.NotNull(selectedProject);
            Assert.Equal(selectedProject.OwnerId, project.OwnerId);
            Assert.Equal(selectedProject.Title, project.Title);
            Assert.Equal(selectedProject.Description, project.Description);
        }
    }
}