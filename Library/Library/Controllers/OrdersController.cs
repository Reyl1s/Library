using BusinessLayer.Interfaces;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class OrdersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IOrderService orderService;

        const string client = "Клиент";
        const string librarian = "Библиотекарь";

        public OrdersController(UserManager<User> userManager, IOrderService orderService)
        {
            this.userManager = userManager;
            this.orderService = orderService;
        }

        // Бронирование книги.
        public async Task<IActionResult> Booking(long id)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            orderService.CreateOrder(id, user);

            return RedirectToAction("UserOrders");
        }

        // Брони клиента.
        [Authorize(Roles = client)]
        public IActionResult UserOrders()
        {
            var userId = userManager.GetUserId(HttpContext.User);
            var orders = orderService.GetUserOrders(userId);

            return View(orders);
        }

        // Все брони.
        [Authorize(Roles = librarian)]
        public IActionResult AllOrders()
        {
            var orders = orderService.GetAllOrders();

            return View(orders);
        }

        // Отмена брони/возвращение книги.
        public ActionResult Delete(long id)
        {
            orderService.DeleteOrder(id);

            if (this.User.IsInRole(client))
            {
                return RedirectToAction("UserOrders");
            }
            else
            {
                return RedirectToAction("AllOrders");
            }
        }

        // Отчача книги клиенту.
        public ActionResult Pass(long id)
        {
            orderService.PassBook(id);

            return RedirectToAction("AllOrders");
        }
    }
}
