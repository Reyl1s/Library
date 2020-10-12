using BuisnessLayer.Interfaces;
using DataLayer.Entities;
using DataLayer.Enums;
using DataLayer.Interfaces;
using Quartz;

namespace BuisnessLayer.Services
{
    [DisallowConcurrentExecution]
    public class OrderChecker : IOrderChecker
    {
        private readonly IRepository<Order> orderRepository;
        private readonly IRepository<Book> bookRepository;

        public OrderChecker(IRepository<Order> orderRepository, IRepository<Book> bookRepository)
        {
            this.orderRepository = orderRepository;
            this.bookRepository = bookRepository;
        }

        public void CheckOrder(Order order)
        {
            var book = bookRepository.Get(order.BookId);
            orderRepository.Delete(order);
            book.BookStatus = BookStatus.Available;
            bookRepository.Update(book);
        }
    }
}
