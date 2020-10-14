using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [Authorize(Roles = admin)]
    public class RolesController : Controller
    {
        const string admin = "Администратор";
        private readonly IUserService userService;

        public RolesController(IUserService userService)
        {
            this.userService = userService;
        }

        // Все роли.
        public IActionResult Index() => View(userService.GetRoles());

        // View создание роли.
        public IActionResult Create() => View();

        // POST создание роли.
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var result = await userService.CreateRoleAsync(name);
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
            return View(name);
        }

        // Удаление роли.
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await userService.DeleteRoleAsync(id);

            return RedirectToAction("Index");
        }

        // Список всех пользователей.
        public IActionResult UserList() => View(userService.GetUsers());

        // GET редактирование пользовательских ролей.
        public async Task<IActionResult> Edit(string userId)
        {
            var model = await userService.EditRoleGet(userId);

            return View(model);
        }

        // POST редактирование пользовательских ролей.
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            await userService.EditRolePost(userId, roles);

            return RedirectToAction("UserList");
        }
    }
}
