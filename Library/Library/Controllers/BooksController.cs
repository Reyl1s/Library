using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    [Authorize(Roles = "Библиотекарь")]
    public class BooksController : Controller
    {
        readonly LibraryDbContext db;
        public BooksController(LibraryDbContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Books.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            db.Books.Add(book);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
