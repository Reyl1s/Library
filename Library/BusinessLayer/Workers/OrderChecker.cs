using DataLayer.Entities;
using DataLayer.Enums;
using DataLayer.Interfaces;
using Quartz;

namespace BuisnessLayer.Workers
{
    [DisallowConcurrentExecution]
    public class OrderChecker : IOrderChecker
    {
        private readonly IOrderRepository<Order> orderRepository;
        private readonly IBookRepository<Book> bookRepository;

        public OrderChecker(IOrderRepository<Order> orderRepository, IBookRepository<Book> bookRepository)
        {
            this.orderRepository = orderRepository;
            this.bookRepository = bookRepository;
        }

        public void CheckOrder(Order order)
        {
            var book = bookRepository.GetBook(order.BookId);
            orderRepository.DeleteOrder(order, book);
        }
    }
}
