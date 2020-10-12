using BusinessLayer.Models.BookDTO;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IBookService
    {
        void CreateBook(BookViewModel bookModel, string path, string uploadedFileName);

        BookViewModel GetBook(long id);

        BooksListViewModel GetBooks();

        public EditBookViewModel EditBookGet(long id);

        public void EditBookPost(EditBookViewModel bookModel, string path, string uploadedFileName);

        void Delete(long id);

        Task<BookSearchViewModel> BookSearchAsync(string searchString,
                                                string bookGenre,
                                                string bookAuthor,
                                                string bookPublisher);
    }
}
