using DataLayer.Entities;
using System;

namespace Library.Models
{
    public class OrdersViewModel
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public long BookId { get; set; }

        public Book Book { get; set; }

        public DateTime DateBooking { get; set; }
    }
}
