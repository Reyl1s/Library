using BusinessLayer.Interfaces;
using BusinessLayer.Models.UserDTO;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        const string client = "Клиент";

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterViewModel model)
        {
            var user = new User
            {
                Email = model.Email,
                Name = model.Name,
                Year = model.Year,
                Phone = model.Phone
            };
            var result = await userManager.CreateAsync(user, model.Password);
            await userManager.AddToRolesAsync(user, new List<string> { client });

            return result;
        }

        public async Task SignInAsync(RegisterViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            await signInManager.SignInAsync(user, false);
        }

        public async Task<RegisterViewModel> GetUserModelByEmail(string email)
        {
            var normalizedEmail = email.ToUpper();
            var user = await userManager.FindByEmailAsync(normalizedEmail);
            var model = new RegisterViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Year = user.Year,
                Phone = user.Phone,
                Email = user.Email
            };

            return model;
        }

        public async Task<SignInResult> PasswordSignInAsync(LoginViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            return result;
        }

        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }
    }
}
