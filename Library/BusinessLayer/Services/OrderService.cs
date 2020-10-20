using BuisnessLayer.Models.OrderDTO;
using BusinessLayer.Interfaces;
using DataLayer.Entities;
using DataLayer.Enums;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Book> bookRepository;
        private readonly IRepository<Order> orderRepository;

        public OrderService(IRepository<Book> bookRepository, IRepository<Order> orderRepository)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
        }

        public void CreateOrder(long id, User user)
        {
            var book = bookRepository.Get(id);
            var order = new Order
            {
                BookId = book.Id,
                UserId = user.Id,
                DateSend = DateTime.Now.AddDays(7),
                DateBooking = DateTime.Now,
                OrderStatus = OrderStatus.Booked

            };
            orderRepository.Create(order);
            book.BookStatus = BookStatus.Booked;
            bookRepository.Update(book);
        }

        public IEnumerable<OrdersViewModel> GetUserOrders(string userId)
        {
            var orders = orderRepository.GetItems()
                .Include(b => b.Book)
                .Where(b => b.UserId == userId)
                .Where(b => b.OrderStatus == OrderStatus.Booked)
                .OrderBy(b => b.Book.Name)
                .Select(b => new OrdersViewModel
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    User = b.User,
                    BookId = b.BookId,
                    Book = b.Book,
                    DateSend = b.DateSend
                })
                .ToList();

            return orders;
        }

        public IEnumerable<OrdersViewModel> GetAllOrders()
        {
            var orders = orderRepository.GetItems()
                .Include(b => b.Book)
                .Include(b => b.User)
                .Where(b => b.OrderStatus == OrderStatus.Booked)
                .OrderBy(b => b.Book.Name)
                .Select(b => new OrdersViewModel
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    User = b.User,
                    BookId = b.BookId,
                    Book = b.Book,
                    DateSend = b.DateSend
                })
                .ToList();

            return orders;
        }

        public void DeleteOrder(long id)
        {
            var order = orderRepository.Get(id);
            var book = bookRepository.Get(order.BookId);
            if (book.BookStatus == BookStatus.Booked)
            {
                order.OrderStatus = OrderStatus.Cancelled;
                order.DateSend = DateTime.Now;
            }
            else if(book.BookStatus == BookStatus.Passed)
            {
                order.OrderStatus = OrderStatus.Returned;
                order.DateSend = DateTime.Now;
            }
            book.BookStatus = BookStatus.Available;
            bookRepository.Update(book);
        }

        public void PassBook(long id)
        {
            var order = orderRepository.Get(id);
            var book = bookRepository.Get(order.BookId);
            book.BookStatus = BookStatus.Passed;
            bookRepository.Update(book);
        }

        public void CheckOrder(Order order)
        {
            var book = bookRepository.Get(order.BookId);
            order.OrderStatus = OrderStatus.Cancelled;
            book.BookStatus = BookStatus.Available;
            bookRepository.Update(book);
        }
    }
}
