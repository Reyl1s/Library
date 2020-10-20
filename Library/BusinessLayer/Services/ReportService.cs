using BusinessLayer.Interfaces;
using BusinessLayer.Models.OrderDTO;
using DataLayer.Entities;
using DataLayer.Enums;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ReportService : IReportService
    {
        private readonly IRepository<Book> bookRepository;
        private readonly IRepository<Order> orderRepository;

        public ReportService(IRepository<Book> bookRepository, IRepository<Order> orderRepository)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<OrdersListViewModel> ReportSearchAsync(string orderStatus, int timeInterval)
        {
            var orders = orderRepository.GetItems();

            if (orderStatus == "Booked")
            {
                orders = orders.Where(x => x.OrderStatus == OrderStatus.Booked);
            }
            if (orderStatus == "Cancelled")
            {
                orders = orders.Where(x => x.OrderStatus == OrderStatus.Cancelled);
            }
            if (orderStatus == "Returned")
            {
                orders = orders.Where(x => x.OrderStatus == OrderStatus.Returned);
            }
            
            if (timeInterval == 1) // За день.
            {
                orders = orders.Where(x => x.DateBooking.AddDays(1) >= DateTime.Now);
            }
            if (timeInterval == 2) // За неделю.
            {
                orders = orders.Where(x => x.DateBooking.AddDays(7) >= DateTime.Now);
            }
            if (timeInterval == 3) // За месяц.
            {
                orders = orders.Where(x => x.DateBooking.AddMonths(1) >= DateTime.Now);
            }

            var orderSearchVM = new OrdersListViewModel
            {
                Orders = await orders.Include(x => x.Book).Include(x => x.User).ToListAsync()
            };

            return orderSearchVM;
        }
    }
}
