using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            const string adminEmail = "admin@gmail.com";
            const string adminPassword = "password";
            const string adminName = "Администратор";
            const string librarianEmail = "librarian@gmail.com";
            const string librarianPassword = "password";
            const string librarianName = "Библиотекарь";
            const string clientName = "Клиент";

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
                User admin = new User 
                { 
                    Email = adminEmail, 
                    UserName = adminEmail 
                };

                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, adminName);
                }
            }
            if (await userManager.FindByNameAsync(librarianEmail) == null)
            {
                User librarian = new User 
                { 
                    Email = librarianEmail, 
                    UserName = librarianEmail 
                };

                IdentityResult result = await userManager.CreateAsync(librarian, librarianPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(librarian, librarianName);
                }
            }
        }
    }
}
