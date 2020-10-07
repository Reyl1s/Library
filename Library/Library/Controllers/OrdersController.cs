using DataLayer.Entities;
using DataLayer.Enums;
using DataLayer.Interfaces;
using BuisnessLayer.Models;
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
        private readonly IBookRepository<Book> bookRepository;
        private readonly IOrderRepository<Order> orderRepository;
        private readonly UserManager<User> userManager;

        public OrdersController
            (UserManager<User> userManager,
            IBookRepository<Book> bookRepository,
            IOrderRepository<Order> orderRepository)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.userManager = userManager;
        }
        
        public async Task<IActionResult> Booking(long id)
        {
            var book = bookRepository.GetBook(id);
            book.BookStatus = BookStatus.Booked;
            bookRepository.UpdateBook(book);

            var user = await userManager.GetUserAsync(HttpContext.User);

            var order = new Order
            {
                BookId = book.Id,
                UserId = user.Id,
                DateBooking = DateTime.Now.AddMinutes(1)

            };
            orderRepository.CreateOrder(order);
            user.UserOrders.Add(order);
            await userManager.UpdateAsync(user);
            

            return RedirectToAction("UserOrders");
        }

        public IActionResult UserOrders()
        {
            var userId = userManager.GetUserId(HttpContext.User);

            var orders = orderRepository.GetOrders()
                .Include(b => b.Book)
                .Where(b => b.UserId == userId)
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

        public IActionResult AllOrders()
        {
            var orders = orderRepository.GetOrders()
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
            var order = orderRepository.GetOrder(id);

            var book = bookRepository.GetBook(order.BookId);
            book.BookStatus = BookStatus.Available;
            bookRepository.UpdateBook(book);

            orderRepository.DeleteOrder(order);

            return RedirectToAction("UserOrders");
        }

        //public ActionResult Subscribe()
        //{
        //    return View();
        //}

        public ActionResult Pass(long id)
        {
            var order = orderRepository.GetOrder(id);

            var book = bookRepository.GetBook(order.BookId);
            book.BookStatus = BookStatus.Passed;
            bookRepository.UpdateBook(book);

            return RedirectToAction("AllOrders");
        }

        public ActionResult Return(long id)
        {
            var order = orderRepository.GetOrder(id);

            var book = bookRepository.GetBook(order.BookId);
            book.BookStatus = BookStatus.Available;
            bookRepository.UpdateBook(book);

            orderRepository.DeleteOrder(order);

            return RedirectToAction("AllOrders");
        }
    }
}
