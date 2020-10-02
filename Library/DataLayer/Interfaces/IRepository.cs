using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Interfaces
{
    public interface IRepository<TEntity>
         where TEntity : class, IEntity
    {
        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(IEnumerable<TEntity> entities);

        IQueryable<TEntity> GetItems();

        TEntity Get(long id);
    }
}
