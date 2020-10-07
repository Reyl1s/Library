using DataLayer.Entities;
using DataLayer.Enums;
using DataLayer.Interfaces;
using System;
using System.Threading.Tasks;

namespace BuisnessLayer.Workers
{
    public class OrderChecker : IOrderChecker
    {
        private readonly IOrderRepository<Order> orderRepository;
        private readonly IBookRepository<Book> bookRepository;

        public OrderChecker
            (IOrderRepository<Order> orderRepository,
            IBookRepository<Book> bookRepository)
        {
            this.orderRepository = orderRepository;
            this.bookRepository = bookRepository;
        }

        public async Task CheckOrder(Order order)
        {
            var book = bookRepository.GetBook(order.BookId);
            if (book.BookStatus != BookStatus.Passed && order.DateBooking < DateTime.Now)
            {
                await orderRepository.DeleteOrderAsync(order);
            }
        }
    }
}
