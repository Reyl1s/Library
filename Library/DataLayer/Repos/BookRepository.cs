using DataLayer.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repos
{
    public class BookRepository<TEntity> : IBookRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public void CreateBook(TEntity entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public void DeleteBook(TEntity entity)
        {
            if (entity != null)
            {
                _context.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void DeleteBooks(IEnumerable<TEntity> entities)
        {
            if (entities.Count() > 0)
            {
                _context.RemoveRange(entities);
                _context.SaveChanges();
            }
        }

        public IQueryable<TEntity> GetBooks()
        {
            return _context.Set<TEntity>();
        }

        public void UpdateBook(TEntity entity)
        {
            _context.SaveChanges();
        }

        public TEntity GetBook(long id)
        {
            return _context.Set<TEntity>().Find(id);
        }
    }
}
