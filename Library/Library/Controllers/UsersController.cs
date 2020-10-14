using BusinessLayer.Interfaces;
using BusinessLayer.Models.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [Authorize(Roles = admin)]
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        const string admin = "Администратор";

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        // Список всех пользователей.
        public IActionResult Index() => View(userService.GetUsers());

        // View создание нового пользователя.
        public IActionResult Create() => View();

        // POST создание нового пользователя.
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.CreateUserAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }

        // GET редактирование пользователя.
        public async Task<IActionResult> Edit(string id)
        {
            var model = await userService.EditUserGet(id);

            return View(model);
        }

        // POST редактирование пользователя.
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.EditUserPost(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
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

        // Удаление пользователя.
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            await userService.DeleteUserAsync(id);

            return RedirectToAction("Index");
        }

        // GET изменение пароля пользователя.
        public async Task<IActionResult> ChangePassword(string id)
        {
            var model = await userService.ChangePasswordGet(id);

            return View(model);
        }

        // POST изменение пароля пользователя.
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.ChangePasswordPost(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
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
    }
}
