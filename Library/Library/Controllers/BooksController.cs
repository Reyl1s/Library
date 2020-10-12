using BusinessLayer.Interfaces;
using BusinessLayer.Models.BookDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Library.Controllers
{

    public class BooksController : Controller
    {
        private readonly IBookService bookService;
        private readonly IWebHostEnvironment _appEnvironment;
        const string librarian = "Библиотекарь";

        public BooksController(IBookService bookService, IWebHostEnvironment appEnvironment)
        {
            this.bookService = bookService;
            _appEnvironment = appEnvironment;
        }

        // Фильтрация книг.
        public async Task<IActionResult> Index(string genre, string author, string publisher, string searchString)
        {
            var bookSearchVM = await bookService.BookSearchAsync(genre, author, publisher, searchString);

            return View(bookSearchVM);
        }

        // GET создание книги.
        [Authorize(Roles = librarian)]
        public IActionResult Create()
        {
            return View();
        }

        // POST создание книги.
        [Authorize(Roles = librarian)]
        [HttpPost]
        public async Task<IActionResult> CreateBook(BookViewModel book, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                var uploadedFileName = uploadedFile.FileName;
                var path = "/Files/" + uploadedFileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                bookService.CreateBook(book, path, uploadedFileName);
            }

            return RedirectToAction("Index");
        }

        // GET редактирование книги.
        [Authorize(Roles = librarian)]
        [HttpGet]
        public IActionResult Edit(long id)
        {
            var model = bookService.EditBookGet(id);

            return View(model);
        }

        // POST редактирование книги.
        [Authorize(Roles = librarian)]
        [HttpPost]
        public async Task<IActionResult> Edit(EditBookViewModel bookModel, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                var uploadedFileName = uploadedFile.FileName;
                var path = "/Files/" + uploadedFileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                bookService.EditBookPost(bookModel, path, uploadedFileName);
            }

            return RedirectToAction("Index");
        }

        // Удаление книги.
        [Authorize(Roles = librarian)]
        public ActionResult Delete(long id)
        {
            bookService.Delete(id);

            return RedirectToAction("Index");
        }

        // Подробнее о книге.
        public IActionResult Details(long id)
        {
            BookViewModel book = bookService.GetBook(id);

            return View(book);
        }
    }
}
