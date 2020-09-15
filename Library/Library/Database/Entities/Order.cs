using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Database.Entities
{
    public class Order
    {
        public long OrderId { get; set; }

        public User User { get; set; }

        public long BookId { get; set; }

        public virtual Book Book { get; set; }
    }
}
