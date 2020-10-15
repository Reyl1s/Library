using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService userService;
        private readonly string client = "Клиент";

        public HomeController(IUserService userService, ILogger<HomeController> logger)
        {
            this.userService = userService;
            _logger = logger;
        }

        // Главная страница юзера.
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(client))
            {
                var user = await userService.GetUserModelAsync(HttpContext.User);
                ViewBag.UserName = user.Name;
            }
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
