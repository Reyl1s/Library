using BuisnessLayer.Models.BookDTO;
using DataLayer.Entities;
using DataLayer.Enums;
using DataLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{

    public class BooksController : Controller
    {
        private readonly IBookRepository<Book> bookRepository;
        private readonly IWebHostEnvironment _appEnvironment;
        const string librarian = "Библиотекарь";

        public BooksController
            (IBookRepository<Book> bookRepository, 
            IWebHostEnvironment appEnvironment)
        {
            this.bookRepository = bookRepository;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index
            (string bookGenre, 
            string bookAuthor,
            string bookPublisher,
            string searchString)
        {
            IQueryable<string> genreQuery = bookRepository
                                            .GetBooks()
                                            .OrderBy(b => b.Genre)
                                            .Select(b => b.Genre);

            IQueryable<string> authorQuery = bookRepository
                                            .GetBooks()
                                            .OrderBy(b => b.Author)
                                            .Select(b => b.Author);

            IQueryable<string> publisherQuery = bookRepository
                                            .GetBooks()
                                            .OrderBy(b => b.Publisher)
                                            .Select(b => b.Publisher);

            var books = bookRepository.GetBooks();

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

            return View(bookSearchVM);
        }

        [Authorize(Roles = librarian)]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = librarian)]
        [HttpPost]
        public async Task<IActionResult> CreateBook(Book book, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/Files/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                var bookModel = new Book
                { 
                    Id = book.Id, 
                    Name = book.Name, 
                    Genre = book.Genre, 
                    Author = book.Author,
                    Publisher = book.Publisher,
                    Description = book.Description,
                    Img = uploadedFile.FileName, 
                    ImgPath = path,
                    BookStatus = BookStatus.Available
                };

                bookRepository.CreateBook(bookModel);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = librarian)]
        [HttpGet]
        public IActionResult Edit(long id)
        {
            var book = bookRepository.GetBook(id);
            if (book == null)
            {
                return NotFound();
            }

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
            return View(model);
        }

        [Authorize(Roles = librarian)]
        [HttpPost]
        public async Task<IActionResult> Edit(EditBookViewModel model, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/Files/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                var book = bookRepository.GetBook(model.Id);
                if (book != null)
                {
                    book.Name = model.Name;
                    book.Genre = model.Genre;
                    book.Author = model.Author;
                    book.Publisher = model.Publisher;
                    book.Description = model.Description;
                    book.Img = uploadedFile.FileName;
                    book.ImgPath = path;

                    bookRepository.UpdateBook(book);

                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [Authorize(Roles = librarian)]
        public ActionResult Delete(long id)
        {
            var book = bookRepository.GetBook(id);
            bookRepository.DeleteBook(book);

            return RedirectToAction("Index");
        }

        public IActionResult Details(long id)
        {
            var book = bookRepository.GetBook(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
    }
}
