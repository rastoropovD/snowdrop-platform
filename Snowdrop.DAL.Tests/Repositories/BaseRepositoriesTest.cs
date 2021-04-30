using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Snowdrop.DAL.Repositories;
using Snowdrop.DAL.Tests.Core;
using Snowdrop.Data.Entites.Core;
using Xunit;

namespace Snowdrop.DAL.Tests.Repositories
{
    public abstract class BaseRepositoriesTest<TEntity> : BaseTest where TEntity : BaseEntity
    {
        protected readonly IRepository<TEntity> Repository = default;
        
        protected BaseRepositoriesTest()
        {
            Repository = Services.GetService<IRepository<TEntity>>();
        }

        [Fact]
        public void GetService_Repository_NotNull()
        {
            Assert.NotNull(Repository);
        }
        
        [Fact]
        public async Task Insert_NullEntity_ArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => Repository.Insert(null));
        }
        
        [Fact]
        public async Task Update_NullEntity_ArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => Repository.Update(null));
        }
        
        [Fact]
        public async Task Delete_NullEntity_ArgumentNullException()
        {
            //Arrange
            int id = -1;

            //Act
            TEntity entity = await Repository.GetSingle(id);

            //Assert
            Assert.Null(entity);
            await Assert.ThrowsAsync<ArgumentNullException>(() => Repository.Delete(id));
        }
    }
}