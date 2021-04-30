using System.Threading.Tasks;
using Snowdrop.Data.Entites.Core;

namespace Snowdrop.DAL.Repositories
{
    public interface IRepository<TEntity>  where TEntity : BaseEntity
    {
        Task<TEntity> GetSingle(int id);
        Task Insert(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(int id);
    }
}