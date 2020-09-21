using Library.Database.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Library.Database.Entities
{
    public class Book
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Genre { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public BookStatus BookStatus { get; set; }
    }
}
