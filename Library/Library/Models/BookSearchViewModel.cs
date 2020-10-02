using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Library.Models
{
    public class BookSearchViewModel
    {
        public List<DataLayer.Entities.Book> Books { get; set; }

        public SelectList Genres { get; set; }

        public string BookGenre { get; set; }

        public SelectList Authors { get; set; }

        public string BookAuthor { get; set; }

        public SelectList Publisher { get; set; }

        public string BookPublisher { get; set; }

        public string SearchString { get; set; }
    }
}
