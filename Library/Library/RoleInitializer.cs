using Library.Database.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminEmail = "admin@gmail.com";
            var adminPassword = "password";
            var adminName = "Администратор";
            var librarianEmail = "librarian@gmail.com";
            var librarianPassword = "password";
            var librarianName = "Библиотекарь";
            var clientName = "Клиент";

            if (await roleManager.FindByNameAsync(adminName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(adminName));
            }
            if (await roleManager.FindByNameAsync(librarianName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(librarianName));
            }
            if (await roleManager.FindByNameAsync(clientName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(clientName));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, adminName);
                }
            }
            if (await userManager.FindByNameAsync(librarianEmail) == null)
            {
                User librarian = new User { Email = librarianEmail, UserName = librarianEmail };
                IdentityResult result = await userManager.CreateAsync(librarian, librarianPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(librarian, librarianName);
                }
            }
        }
    }
}
