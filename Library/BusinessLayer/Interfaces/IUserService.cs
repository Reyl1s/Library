using BusinessLayer.Models.UserDTO;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IUserService
    {
        public Task<IdentityResult> CreateUserAsync(RegisterViewModel model);

        public Task<IdentityResult> CreateUserAsync(CreateUserViewModel model);

        public Task<RegisterViewModel> GetUserModelByEmail(string Email);

        public  Task SignInAsync(RegisterViewModel model);

        public Task<SignInResult> PasswordSignInAsync(LoginViewModel model);

        public Task SignOutAsync();

        public List<IdentityRole> GetRoles();

        public Task<IdentityResult> CreateRoleAsync(string name);

        public Task<IdentityRole> FindRoleByIdAsync(string id);

        public Task DeleteRoleAsync(string id);

        public List<User> GetUsers();

        public Task<ChangeRoleViewModel> EditRoleGet(string id);

        public Task EditRolePost(string id, List<string> roles);

        public Task<EditUserViewModel> EditUserGet(string id);

        public Task<IdentityResult> EditUserPost(EditUserViewModel model);

        public Task DeleteUserAsync(string id);

        public Task<ChangePasswordViewModel> ChangePasswordGet(string id);

        public Task<IdentityResult> ChangePasswordPost(ChangePasswordViewModel model);

        public Task<UserViewModel> GetUserModelAsync(ClaimsPrincipal HttpUserContext);
    }
}
