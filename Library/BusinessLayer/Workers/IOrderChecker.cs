using DataLayer.Entities;

namespace BuisnessLayer.Workers
{
    public interface IOrderChecker
    {
        void CheckOrder(Order order);
    }
}
