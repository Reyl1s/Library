using DataLayer.Entities;
using DataLayer.Interfaces;
using BuisnessLayer.Workers;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Threading.Tasks;

namespace BuisnessLayer.Jobs
{
    public class DataJob : IJob
    {
        private readonly IOrderRepository<Order> orderRepository;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public DataJob
            (IOrderRepository<Order> orderRepository,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.orderRepository = orderRepository;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var orderChecker = scope.ServiceProvider.GetService<IOrderChecker>();
                var orders = orderRepository.GetOrders();
                foreach (var order in orders)
                {
                    await orderChecker.CheckOrder(order);
                }
            }
        }
    }
}
