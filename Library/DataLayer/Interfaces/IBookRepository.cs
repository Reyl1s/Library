using DataLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Interfaces
{
    public interface IBookRepository<TEntity>
         where TEntity : class, IEntity
    {
        void CreateBook(TEntity entity);

        void UpdateBook(TEntity entity);

        void PassBook(Book book);

        void DeleteBook(TEntity entity);

        void DeleteBooks(IEnumerable<TEntity> entities);

        IQueryable<TEntity> GetBooks();

        TEntity GetBook(long id);
    }
}
