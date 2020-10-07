using DataLayer.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repos
{
    public class OrderRepository<TEntity> : IOrderRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly LibraryDbContext _context;

        public OrderRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public void CreateOrder(TEntity entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public void DeleteOrder(TEntity entity)
        {
            if (entity != null)
            {
                _context.Remove(entity);
                _context.SaveChanges();
            }
        }

        public async Task DeleteOrderAsync(TEntity entity)
        {
            if (entity != null)
            {
                _context.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public void DeleteOrders(IEnumerable<TEntity> entities)
        {
            if (entities.Count() > 0)
            {
                _context.RemoveRange(entities);
                _context.SaveChanges();
            }
        }

        public IQueryable<TEntity> GetOrders()
        {
            return _context.Set<TEntity>();
        }

        public void UpdateOrder(TEntity entity)
        {
            _context.SaveChanges();
        }

        public TEntity GetOrder(long id)
        {
            return _context.Set<TEntity>().Find(id);
        }
    }
}
