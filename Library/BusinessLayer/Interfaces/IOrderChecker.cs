using DataLayer.Entities;

namespace BuisnessLayer.Interfaces
{
    public interface IOrderChecker
    {
        void CheckOrder(Order order);
    }
}
