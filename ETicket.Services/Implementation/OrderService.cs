using ETicket.Domain.DomainModels;
using ETicket.Repository;
using ETicket.Repository.Interface;
using ETicket.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

       // private readonly ApplicationDbContext context;
        //private DbSet<Order> entities;

        public OrderService(IOrderRepository orderRepository) {

            _orderRepository = orderRepository;

            //this.context = context;
            //entities = context.Set<Order>();
        }

        public List<Order> GetAllOrders()
        {
            return this._orderRepository.GetAllOrders();
        }

        public Order GetOrderDetails(BaseEntity model) {

            return this._orderRepository.GetOrderDetails(model);
        }
    }
}
