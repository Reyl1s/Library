using BusinessLayer.Models.UserDTO;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;

        public AccountController (IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //User user = new User 
                //{ 
                //    Email = model.Email,
                //    UserName = model.Email,
                //    Name = model.Name,
                //    Phone = model.Phone,
                //    Year = model.Year 
                //};

                //var result = await _userManager.CreateAsync(user, model.Password);
                //await _userManager.AddToRolesAsync(user, new List<string> { client });

                var result = await userService.CreateUserAsync(model);
                if (result.Succeeded)
                {
                    var userModel = await userService.GetUserModelByEmail(model.Email);
                    await userService.SignInAsync(userModel);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                var result = await userService.PasswordSignInAsync(model);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await userService.SignOutAsync();
            return RedirectToAction("Index", "Books");
        }
    }
}
