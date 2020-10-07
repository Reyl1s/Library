using DataLayer.Entities;
using System.Threading.Tasks;

namespace BuisnessLayer.Workers
{
    public interface IOrderChecker
    {
        Task CheckOrder(Order order);
    }
}
