using DataLayer.Entities;
using DataLayer.Enums;
using DataLayer.Interfaces;
using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IRepository<Book> bookRepository;
        private readonly IRepository<Order> orderRepository;
        private readonly UserManager<User> userManager;

        public OrdersController
            (UserManager<User> userManager,
            IRepository<Book> bookRepository,
            IRepository<Order> orderRepository)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.userManager = userManager;
        }
        
        public async Task<IActionResult> Booking(long id)
        {
            var book = bookRepository.Get(id);
            book.BookStatus = BookStatus.Booked;
            bookRepository.Update(book);

            var user = await userManager.GetUserAsync(HttpContext.User);

            var order = new Order
            {
                BookId = book.Id,
                UserId = user.Id,
                DateBooking = DateTime.Now.AddDays(7)

            };
            orderRepository.Create(order);
            user.UserOrders.Add(order);
            await userManager.UpdateAsync(user);
            

            return RedirectToAction("UserOrders");
        }

        public async Task<IActionResult> UserOrders()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            
            var orders = await orderRepository.GetItems()
                .Include(b => b.Book)
                .Where(b => b.UserId == user.Id)
                .OrderBy(b => b.Book.Name)
                .Select(b => new OrdersViewModel
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    User = b.User,
                    BookId = b.BookId,
                    Book = b.Book,
                    DateBooking = b.DateBooking
                })
                .ToListAsync();

            return View(orders);
        }

        public IActionResult AllOrders()
        {
            var orders = orderRepository.GetItems()
                .Include(b => b.Book)
                .Include(b => b.User)
                .OrderBy(b => b.Book.Name)
                .Select(b => new OrdersViewModel
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    User = b.User,
                    BookId = b.BookId,
                    Book = b.Book,
                    DateBooking = b.DateBooking
                })
                .ToList();

            return View(orders);
        }

        public ActionResult Delete(long id)
        {
            var order = orderRepository.Get(id);

            var book = bookRepository.Get(order.BookId);
            book.BookStatus = BookStatus.Available;
            bookRepository.Update(book);

            orderRepository.Delete(order);

            return RedirectToAction("UserOrders");
        }

        //public ActionResult Subscribe()
        //{
        //    return View();
        //}

        public ActionResult Pass(long id)
        {
            var order = orderRepository.Get(id);

            var book = bookRepository.Get(order.BookId);
            book.BookStatus = BookStatus.Passed;
            bookRepository.Update(book);

            return RedirectToAction("AllOrders");
        }

        public ActionResult Return(long id)
        {
            var order = orderRepository.Get(id);

            var book = bookRepository.Get(order.BookId);
            book.BookStatus = BookStatus.Available;
            bookRepository.Update(book);

            orderRepository.Delete(order);

            return RedirectToAction("AllOrders");
        }
    }
}
