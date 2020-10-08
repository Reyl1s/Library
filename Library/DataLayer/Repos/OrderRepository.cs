using DataLayer.Entities;
using DataLayer.Enums;
using DataLayer.Interfaces;
using System.Collections.Generic;
using System.Linq;

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

        public void CreateOrder(TEntity entity, Book book)
        {
            book.BookStatus = BookStatus.Booked;
            _context.Update(book);
            _context.Add(entity);
            _context.SaveChanges();
        }

        public void DeleteOrder(TEntity entity, Book book)
        {
            if (entity != null)
            {
                book.BookStatus = BookStatus.Available;
                _context.Update(book);
                _context.Remove(entity);
                _context.SaveChanges();
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
