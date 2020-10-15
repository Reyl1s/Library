using BusinessLayer.Interfaces;
using BusinessLayer.Models.UserDTO;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        const string client = "Клиент";

        public UserService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterViewModel model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
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

        public List<IdentityRole> GetRoles()
        {
            var roles = roleManager.Roles.ToList();

            return roles;
        }

        public async Task<IdentityResult> CreateRoleAsync(string name)
        {
            var result = await roleManager.CreateAsync(new IdentityRole(name));

            return result;
        }

        public async Task<IdentityRole> FindRoleByIdAsync(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            return role;
        }

        public async Task DeleteRoleAsync(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                await roleManager.DeleteAsync(role);
            }
        }

        public List<User> GetUsers()
        {
            var users = userManager.Users.ToList();

            return users;
        }

        public async Task<ChangeRoleViewModel> EditRoleGet(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var userRoles = await userManager.GetRolesAsync(user);
            var allRoles = roleManager.Roles.ToList();
            ChangeRoleViewModel model = new ChangeRoleViewModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserRoles = userRoles,
                AllRoles = allRoles
            };

            return model;
        }

        public async Task EditRolePost(string id, List<string> roles)
        {
            User user = await userManager.FindByIdAsync(id);
            var userRoles = await userManager.GetRolesAsync(user);
            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.Except(roles);
            await userManager.AddToRolesAsync(user, addedRoles);
            await userManager.RemoveFromRolesAsync(user, removedRoles);
        }

        public async Task<EditUserViewModel> EditUserGet(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            EditUserViewModel model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone,
                Year = user.Year
            };

            return model;
        }

        public async Task<IdentityResult> EditUserPost(EditUserViewModel model)
        {
            User user = await userManager.FindByIdAsync(model.Id);
            user.Email = model.Email;
            user.UserName = model.Email;
            user.Name = model.Name;
            user.Phone = model.Phone;
            user.Year = model.Year;
            var result = await userManager.UpdateAsync(user);

            return result;
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }
        }

        public async Task<ChangePasswordViewModel> ChangePasswordGet(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            var model = new ChangePasswordViewModel
            {
                Id = user.Id,
                Email = user.Email
            };

            return model;
        }

        public async Task<IdentityResult> ChangePasswordPost(ChangePasswordViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            return result;
        }

        public async Task<IdentityResult> CreateUserAsync(CreateUserViewModel model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Name,
                Year = model.Year,
                Phone = model.Phone
            };
            var result = await userManager.CreateAsync(user, model.Password);
            await userManager.AddToRolesAsync(user, new List<string> { client });

            return result;
        }

        public async Task<UserViewModel> GetUserModelAsync(ClaimsPrincipal HttpUserContext)
        {
            var user = await userManager.GetUserAsync(HttpUserContext);
            var userModel = new UserViewModel
            {
                Name = user.Name
            };

            return userModel;
        }
    }
}
