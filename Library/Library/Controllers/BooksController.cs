using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Database;
using Library.Database.Entities;
using Library.Database.Enums;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    
    public class BooksController : Controller
    {
        private readonly LibraryDbContext db;

        public BooksController(LibraryDbContext context)
        {
            db = context;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(Book book)
        {
            book.BookStatus = BookStatus.Available;
            db.Books.Add(book);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(long id)
        {
            Book book = db.Books.FirstOrDefault(p => p.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            EditBookViewModel model = new EditBookViewModel { Id = book.Id, Name = book.Name, Genre = book.Genre, Author = book.Author, Publisher = book.Publisher };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                Book book = db.Books.FirstOrDefault(p => p.Id == model.Id);
                if (book != null)
                {
                    book.Name = model.Name;
                    book.Genre = model.Genre;
                    book.Author = model.Author;
                    book.Publisher = model.Publisher;

                    db.Books.Update(book);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public ActionResult Delete(long id)
        {
            Book b = new Book { Id = id };
            db.Entry(b).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
