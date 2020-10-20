using BusinessLayer.Models.OrderDTO;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IReportService
    {
        public Task<OrdersListViewModel> ReportSearchAsync(string orderStatus, int timeInterval);
    }
}
