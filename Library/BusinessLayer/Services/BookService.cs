using BusinessLayer.Interfaces;
using BusinessLayer.Models.BookDTO;
using DataLayer.Entities;
using DataLayer.Enums;
using DataLayer.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> bookRepository;

        public BookService(IRepository<Book> bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public void CreateBook(BookViewModel bookModel, string path, string uploadedFileName)
        {
            var book = new Book
            {
                Name = bookModel.Name,
                Genre = bookModel.Genre,
                Author = bookModel.Author,
                Publisher = bookModel.Publisher,
                Description = bookModel.Description,
                Img = uploadedFileName,
                ImgPath = path,
                BookStatus = BookStatus.Available
            };

            bookRepository.Create(book);
        }

        public BookViewModel GetBook(long id)
        {
            var book = bookRepository.Get(id);
            var bookVM = new BookViewModel
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Genre = book.Genre,
                Publisher = book.Publisher,
                Description = book.Description,
                Img = book.Img,
                ImgPath = book.ImgPath,
                BookStatus = book.BookStatus
            };
            return bookVM;
        }

        public BooksListViewModel GetBooks()
        {
            IQueryable<Book> books = bookRepository.GetItems();
            var booksModel = new BooksListViewModel { Books = books };

            return booksModel;
        }

        public EditBookViewModel EditBookGet(long id)
        {
            var book = bookRepository.Get(id);

            var model = new EditBookViewModel
            {
                Id = book.Id,
                Name = book.Name,
                Genre = book.Genre,
                Author = book.Author,
                Publisher = book.Publisher,
                Description = book.Description,
                Img = book.Img,
                ImgPath = book.ImgPath
            };

            return model;
        }

        public void EditBookPost(EditBookViewModel bookModel, string path, string uploadedFileName)
        {
            var book = bookRepository.Get(bookModel.Id);
            if (book != null)
            {
                book.Id = bookModel.Id;
                book.Name = bookModel.Name;
                book.Genre = bookModel.Genre;
                book.Author = bookModel.Author;
                book.Publisher = bookModel.Publisher;
                book.Description = bookModel.Description;
                book.Img = uploadedFileName;
                book.ImgPath = path;

                bookRepository.Update(book);
            }
        }

        public void Delete(long id)
        {
            var book = bookRepository.Get(id);
            bookRepository.Delete(book);
        }

        public async Task<BookSearchViewModel> BookSearchAsync(string bookGenre, string bookAuthor,
            string bookPublisher, string searchString)
        {
            var genreQuery = bookRepository.GetItems()
                .OrderBy(b => b.Genre)
                .Select(b => b.Genre);

            var authorQuery = bookRepository.GetItems()
                .OrderBy(b => b.Author)
                .Select(b => b.Author);

            var publisherQuery = bookRepository.GetItems()
                .OrderBy(b => b.Publisher)
                .Select(b => b.Publisher);

            var books = bookRepository.GetItems();
            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Name.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(bookGenre))
            {
                books = books.Where(x => x.Genre == bookGenre);
            }
            if (!string.IsNullOrEmpty(bookAuthor))
            {
                books = books.Where(x => x.Author == bookAuthor);
            }
            if (!string.IsNullOrEmpty(bookPublisher))
            {
                books = books.Where(x => x.Publisher == bookPublisher);
            }

            var bookSearchVM = new BookSearchViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Authors = new SelectList(await authorQuery.Distinct().ToListAsync()),
                Publisher = new SelectList(await publisherQuery.Distinct().ToListAsync()),
                Books = await books.OrderBy(b => b.Name).ToListAsync()
            };

            return bookSearchVM;
        }
    }
}
