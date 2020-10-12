using DataLayer.Entities;
using System.Linq;

namespace BusinessLayer.Models.BookDTO
{
    public class BooksListViewModel
    {
        public IQueryable<Book> Books { get; set; }
    }
}
