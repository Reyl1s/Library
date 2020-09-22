using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Database.Entities
{
    public class Order
    {
        public long OrderId { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public long BookId { get; set; }

        public Book Book { get; set; }

        public DateTime DateBooking { get; set; }
    }
}
