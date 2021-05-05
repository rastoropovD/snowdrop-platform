using System.Threading.Tasks;
using Snowdrop.Data.Entites;
using Xunit;

namespace Snowdrop.DAL.Tests.Repositories
{
    public sealed class UserRepositoryTest : BaseRepositoriesTest<User>
    {
        [Fact]
        public async Task Insert_User_UserInserted()
        {
            //Arrange
            int userId = 1;
            User user = new()
            {
                Id = userId,
                Email = "Test",
                PasswordHash = "SomeHash"
            };

            //Act
            await Repository.Insert(user);
            User selectedUser = await Repository.GetSingle(userId);

            //Assert
            Assert.NotNull(selectedUser);
            Assert.Equal(selectedUser.Email, user.Email);
            Assert.Equal(selectedUser.PasswordHash, user.PasswordHash);
        }
    }
}