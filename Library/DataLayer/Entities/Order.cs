using DataLayer.Enums;
using DataLayer.Interfaces;
using System;

namespace DataLayer.Entities
{
    public class Order : IEntity
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public long BookId { get; set; }

        public Book Book { get; set; }

        public DateTime DateSend { get; set; }

        public DateTime DateBooking { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
