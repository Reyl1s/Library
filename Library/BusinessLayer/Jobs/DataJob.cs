﻿using BusinessLayer.Interfaces;
using DataLayer.Entities;
using DataLayer.Enums;
using DataLayer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BuisnessLayer.Jobs
{
    [DisallowConcurrentExecution]
    public class DataJob : IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private static readonly object _locker = new object();

        public DataJob(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public Task Execute(IJobExecutionContext context)
        {
            lock (_locker)
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var orderChecker = scope.ServiceProvider.GetRequiredService<IOrderService>();
                    var orderRepository = scope.ServiceProvider.GetRequiredService<IRepository<Order>>();

                    var now = DateTime.Now;
                    var orders = orderRepository.GetItems()
                        .Where(x => x.DateSend <= now)
                        .Where(x => x.Book.BookStatus == BookStatus.Booked)
                        .ToList();

                    foreach (var order in orders)
                    {
                        orderChecker.CheckOrder(order);
                    }

                    return Task.CompletedTask;
                }
            }
        }
    }
}
