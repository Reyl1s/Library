using DataLayer.Entities;
using DataLayer.Enums;
using System.Collections.Generic;

namespace BusinessLayer.Models.OrderDTO
{
    public class OrdersListViewModel
    {
        public List<Order> Orders { get; set; }

        public string OrderStatus { get; set; }

        public TimeInterval TimeInterval { get; set; }
    }
}
