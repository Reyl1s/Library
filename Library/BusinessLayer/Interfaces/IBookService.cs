using BusinessLayer.Models.BookDTO;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IBookService
    {
        public void CreateBook(BookViewModel bookModel, string path, string uploadedFileName);

        public BookViewModel GetBook(long id);

        public BooksListViewModel GetBooks();

        public EditBookViewModel EditBookGet(long id);

        public void EditBookPost(EditBookViewModel bookModel, string path, string uploadedFileName);

        public void Delete(long id);

        public Task<BookSearchViewModel> BookSearchAsync(string bookGenre,
                                                        string bookAuthor,
                                                        string bookPublisher,
                                                        string searchString);
    }
}
