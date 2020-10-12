using DataLayer.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repos
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly LibraryDbContext _context;

        public Repository(LibraryDbContext context)
        {
            _context = context;
        }

        public void Create(TEntity entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            if (entity != null)
            {
                _context.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            if (entities.Count() > 0)
            {
                _context.RemoveRange(entities);
                _context.SaveChanges();
            }
        }

        public IQueryable<TEntity> GetItems()
        {
            return _context.Set<TEntity>();
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }

        public TEntity Get(long id)
        {
            return _context.Set<TEntity>().Find(id);
        }
    }
}
