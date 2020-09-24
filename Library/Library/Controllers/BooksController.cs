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

        public ActionResult Delete(int id)
        {
            Book b = new Book { Id = id };
            db.Entry(b).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
