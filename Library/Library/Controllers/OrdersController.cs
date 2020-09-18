using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Database;
using Library.Database.Entities;
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
            Order order = new Order { BookId = book.Id, UserId = user.Id, User = user };

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
            return View(await db.Orders.Include(x => x.Book).Include(b => b.User).ToListAsync());
        }

        public ActionResult Delete(int id)
        {
            Order b = new Order { OrderId = id };
            db.Entry(b).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("UserOrders");
        }
    }
}
