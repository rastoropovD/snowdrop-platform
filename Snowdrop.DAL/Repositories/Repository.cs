using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snowdrop.DAL.Context;
using Snowdrop.Data.Entites.Core;

namespace Snowdrop.DAL.Repositories
{
    public sealed class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly SnowdropContext m_context = default;
        private readonly DbSet<TEntity> m_entities = default;
        
        public Repository(IServiceProvider services)
        {
            m_context = services.GetService<SnowdropContext>();

            if (m_context == null)
            {
                throw new NullReferenceException($"Cannot get db context!");
            }

            m_entities = m_context.Set<TEntity>();
        }
        
        public Task<TEntity> GetSingle(int id)
        {
            return m_entities.SingleOrDefaultAsync(entity => entity.Id == id);
        }

        public Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> predicate)
        {
            return m_entities
                   .Where(predicate)
                   .SingleOrDefaultAsync();
        }

        public Task Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)}");
            }
            
            m_entities.Add(entity);
            return m_context.SaveChangesAsync();
        }

        public Task Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)}");
            }
            
            entity.ModifiedAt = DateTime.UtcNow;
            m_entities.Update(entity);
            return m_context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            TEntity entity = await GetSingle(id);
            
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)}");
            }
            
            m_entities.Remove(entity);
            await m_context.SaveChangesAsync();
        }
    }
}