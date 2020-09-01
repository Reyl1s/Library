using System.Linq;
using Library.Models;

namespace Library
{
    public static class SampleData
    {
        public static void Initialize(LibraryContext context)
        {
            if (!context.Books.Any())
            {
                context.Books.AddRange(
                    new Book
                    {
                        Name = "Над пропастью во ржи(Пример)",
                        Genre = "Ужастик(Пример)",
                        Availability = true
                    },
                    new Book
                    {
                        Name = "Великий Гетсби(Пример)",
                        Genre = "Детектив(Пример)",
                        Availability = true
                    },
                    new Book
                    {
                        Name = "Стигматы(Пример)",
                        Genre = "Хоррор(Пример)",
                        Availability = true
                    }
                );
                context.SaveChanges();
            }
        }
    }
}