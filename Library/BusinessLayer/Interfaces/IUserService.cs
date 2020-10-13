using BusinessLayer.Models.UserDTO;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IUserService
    {
        public Task<IdentityResult> CreateUserAsync(RegisterViewModel model);

        public Task<RegisterViewModel> GetUserModelByEmail(string Email);

        public  Task SignInAsync(RegisterViewModel model);

        public Task<SignInResult> PasswordSignInAsync(LoginViewModel model);

        public Task SignOutAsync();
    }
}
