using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Order> Orders { get; set; }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
