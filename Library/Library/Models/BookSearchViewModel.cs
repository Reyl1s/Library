using Library.Database.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class BookSearchViewModel
    {
        public List<Book> Books { get; set; }

        public SelectList Genres { get; set; }

        public string BookGenre { get; set; }

        public SelectList Authors { get; set; }

        public string BookAuthor { get; set; }

        public SelectList Publisher { get; set; }

        public string BookPublisher { get; set; }

        public string SearchString { get; set; }
    }
}
