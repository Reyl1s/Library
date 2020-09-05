using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Database.Entities
{
    public class Order
    {
        public long OrderId { get; set; }

        public string User { get; set; }

        public string ContactPhone { get; set; }


        public int BookId { get; set; }

        public Book Book { get; set; }
    }
}
