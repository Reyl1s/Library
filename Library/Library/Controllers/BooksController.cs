using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Library.Database;
using Library.Database.Entities;
using Library.Database.Enums;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    
    public class BooksController : Controller
    {
        private readonly LibraryDbContext db;

        private readonly IWebHostEnvironment _appEnvironment;

        const string librarian = "Библиотекарь";

        public BooksController(LibraryDbContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index(string bookGenre, string bookAuthor, string bookPublisher, string searchString)
        {
            IQueryable<string> genreQuery = from b in db.Books
                                            orderby b.Genre
                                            select b.Genre;

            IQueryable<string> authorQuery = from b in db.Books
                                            orderby b.Author
                                            select b.Author;

            IQueryable<string> publisherQuery = from b in db.Books
                                            orderby b.Publisher
                                            select b.Publisher;

            var books = from b in db.Books
                         select b;

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
            //book.BookStatus = BookStatus.Available;
            //db.Books.Add(book);
            //await db.SaveChangesAsync();

            if (uploadedFile != null)
            {
                string path = "/Files/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                Book bookModel = new Book 
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

                db.Books.Add(bookModel);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(long id)
        {
            Book book = db.Books.FirstOrDefault(p => p.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            EditBookViewModel model = new EditBookViewModel 
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

                Book book = db.Books.FirstOrDefault(p => p.Id == model.Id);
                if (book != null)
                {
                    book.Name = model.Name;
                    book.Genre = model.Genre;
                    book.Author = model.Author;
                    book.Publisher = model.Publisher;
                    book.Description = model.Description;
                    book.Img = uploadedFile.FileName;
                    book.ImgPath = path;

                    db.Books.Update(book);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        //[HttpPost]
        //public IActionResult Edit(EditBookViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Book book = db.Books.FirstOrDefault(p => p.Id == model.Id);
        //        if (book != null)
        //        {
        //            book.Name = model.Name;
        //            book.Genre = model.Genre;
        //            book.Author = model.Author;
        //            book.Publisher = model.Publisher;
        //            book.Description = model.Description;
        //            book.Img = model.Img;

        //            db.Books.Update(book);
        //            db.SaveChanges();

        //            return RedirectToAction("Index");
        //        }
        //    }
        //    return View(model);
        //}

        [Authorize(Roles = librarian)]
        public ActionResult Delete(long id)
        {
            Book b = new Book { Id = id };
            db.Entry(b).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await db.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
    }
}
