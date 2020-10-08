using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IOrderRepository<TEntity>
         where TEntity : class, IEntity
    {
        void CreateOrder(TEntity entity);

        void UpdateOrder(TEntity entity);

        void DeleteOrder(TEntity entity);

        void DeleteOrders(IEnumerable<TEntity> entities);

        IQueryable<TEntity> GetOrders();

        TEntity GetOrder(long id);
    }
}
