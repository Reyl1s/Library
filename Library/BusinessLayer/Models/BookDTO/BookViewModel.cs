using DataLayer.Enums;

namespace BusinessLayer.Models.BookDTO
{
    public class BookViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Genre { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public string Description { get; set; }

        public string Img { get; set; }

        public string ImgPath { get; set; }

        public BookStatus BookStatus { get; set; }
    }
}
