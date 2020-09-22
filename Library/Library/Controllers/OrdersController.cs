using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Database;
using Library.Database.Entities;
using Library.Database.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class OrdersController : Controller
    {
        private readonly LibraryDbContext db;
        private readonly UserManager<User> _userManager;

        public OrdersController(LibraryDbContext context, UserManager<User> userManager)
        {
            db = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Booking()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Booking(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Book book = db.Books.FirstOrDefault(p => p.Id == id);
            book.BookStatus = BookStatus.Booked;
            Order order = new Order { BookId = book.Id, UserId = user.Id, User = user };
            order.DateBooking = DateTime.Now.AddDays(7);

            db.Orders.Add(order);
            db.SaveChanges();

            return RedirectToAction("UserOrders");
        }

        public async Task<IActionResult> UserOrders()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userId = user.Id;
            var orders = await db.Orders.Include(b => b.Book).Where(b => b.UserId == userId).ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> AllOrders()
        {
            var oldOrders = await db.Orders.Include(a => a.Book).Include(b => b.User).ToListAsync();
            foreach (var order in oldOrders)
            {
                if (order.DateBooking < DateTime.Now)
                {
                    order.Book.BookStatus = BookStatus.Available;
                    db.Entry(order).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            var newOrders = await db.Orders.Include(b => b.Book).Include(b => b.User).ToListAsync();
            return View(newOrders);
        }

        public ActionResult Delete(int id)
        {
            var order = db.Orders.FirstOrDefault(x => x.OrderId == id);
            var bookId = order.BookId;
            Book book = db.Books.FirstOrDefault(p => p.Id == bookId);
            book.BookStatus = BookStatus.Available;

            db.Entry(order).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("UserOrders");
        }

        public ActionResult Subscribe(int id)
        {
            return View();
        }

        public ActionResult Pass(int id)
        {
            var order = db.Orders.FirstOrDefault(x => x.OrderId == id);
            var bookId = order.BookId;
            Book book = db.Books.FirstOrDefault(p => p.Id == bookId);
            book.BookStatus = BookStatus.Passed;

            db.SaveChanges();

            return RedirectToAction("AllOrders");
        }

        public ActionResult Return(int id)
        {
            var order = db.Orders.FirstOrDefault(x => x.OrderId == id);
            var bookId = order.BookId;
            Book book = db.Books.FirstOrDefault(p => p.Id == bookId);
            book.BookStatus = BookStatus.Available;

            db.Entry(order).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("AllOrders");
        }
    }
}
