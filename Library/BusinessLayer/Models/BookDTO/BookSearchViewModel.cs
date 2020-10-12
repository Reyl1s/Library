using System.Collections.Generic;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusinessLayer.Models.BookDTO
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
