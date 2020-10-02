using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public int Year { get; set; }

        public string Phone { get; set; }

        public List<Order> UserOrders { get; set; }
    }
}