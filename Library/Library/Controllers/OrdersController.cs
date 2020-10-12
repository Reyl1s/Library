using BuisnessLayer.Models.OrderDTO;
using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        const string client = "Клиент";
        const string librarian = "Библиотекарь";

        public OrdersController(UserManager<User> userManager,
            IBookRepository<Book> bookRepository,
            IOrderRepository<Order> orderRepository)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.userManager = userManager;
        }

        // Бронирование книги.
        public async Task<IActionResult> Booking(long id)
        {
            var book = bookRepository.GetBook(id);
            var user = await userManager.GetUserAsync(HttpContext.User);
            var order = new Order
            {
                BookId = book.Id,
                UserId = user.Id,
                DateBooking = DateTime.Now.AddDays(7)

            };
            orderRepository.CreateOrder(order, book);
            user.UserOrders.Add(order);
            await userManager.UpdateAsync(user);

            return RedirectToAction("UserOrders");
        }

        // Брони клиента.
        [Authorize(Roles = client)]
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

        // Все брони.
        [Authorize(Roles = librarian)]
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

        // Отмена брони/возвращение книги.
        public ActionResult Delete(long id)
        {
            var order = orderRepository.GetOrder(id);
            var book = bookRepository.GetBook(order.BookId);
            orderRepository.DeleteOrder(order, book);

            if (this.User.IsInRole(client))
            {
                return RedirectToAction("UserOrders");
            }
            else
            {
                return RedirectToAction("AllOrders");
            }
        }

        // Отчача книги клиенту.
        public ActionResult Pass(long id)
        {
            var order = orderRepository.GetOrder(id);
            var book = bookRepository.GetBook(order.BookId);
            bookRepository.PassBook(book);

            return RedirectToAction("AllOrders");
        }
    }
}
