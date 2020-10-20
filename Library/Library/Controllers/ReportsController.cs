using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportService reportService;

        public ReportsController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        public async Task<IActionResult> ReportIndex(string orderStatus, int timeInterval)
        {
            var orderSearchVM = await reportService.ReportSearchAsync(orderStatus, timeInterval);

            return View(orderSearchVM);
        }
    }
}
