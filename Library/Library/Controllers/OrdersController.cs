using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Database;
using Library.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class OrdersController : Controller
    {
        private readonly LibraryDbContext db;

        public OrdersController(LibraryDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Booking(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            ViewBag.BookId = id;
            return View();
        }

        [HttpPost]
        public IActionResult Booking(Order order)
        {

            db.Orders.Add(order);
            // сохраняем в бд все изменения
            db.SaveChanges();
            return RedirectToAction("UserOrders");
        }

        public async Task<IActionResult> UserOrders()
        {
            return View(await db.Orders.Include(x => x.Book).ToListAsync());
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
