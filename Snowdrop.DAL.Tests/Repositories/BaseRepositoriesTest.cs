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
        public void GetService_Repository_InstanceNotNull()
        {
            //Assert
            Assert.NotNull(Repository);
        }
        
        [Fact]
        public async Task Insert_NullEntity_ArgumentNullExceptionThrown()
        {
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => Repository.Insert(null));
        }
        
        [Fact]
        public async Task Update_NullEntity_ArgumentNullExceptionThrown()
        {
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => Repository.Update(null));
        } 
        
        [Fact]
        public async Task Delete_EntityIsNotExist_ArgumentNullExceptionThrown()
        {
            //Arrange
            int id = -1;
            
            //Act
            TEntity entity = await Repository.GetSingle(id);
            
            //Assert
            Assert.Null(entity);
            await Assert.ThrowsAsync<ArgumentNullException>(() => Repository.Delete(-1));
        }
    }
}