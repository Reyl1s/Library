using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Database.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public int Year { get; set; }

        public string Phone { get; set; }
    }
}