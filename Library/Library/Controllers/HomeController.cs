using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Library.Models;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        LibraryDbContext db;
        public HomeController(LibraryDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View(db.Books.ToList());
        }

        [HttpGet]
        public IActionResult Booking(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            ViewBag.BookId = id;
            return View();
        }
        [HttpPost]
        public string Booking(Order order)
        {
            db.Orders.Add(order);
            // сохраняем в бд все изменения
            db.SaveChanges();
            return "Спасибо, " + order.User + ", за бронирование!";
        }
    }
}
