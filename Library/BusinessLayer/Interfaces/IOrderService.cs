using BuisnessLayer.Models.OrderDTO;
using DataLayer.Entities;
using System.Collections.Generic;

namespace BusinessLayer.Interfaces
{
    public interface IOrderService
    {
        public void CreateOrder(long id, User user);

        public IEnumerable<OrdersViewModel> GetUserOrders(string userId);

        public IEnumerable<OrdersViewModel> GetAllOrders();

        public void DeleteOrder(long id);

        public void PassBook(long id);
    }
}
