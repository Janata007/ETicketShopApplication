using ETicket.Domain.DomainModels;
using ETicket.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {

        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepository(ApplicationDbContext context) {

            this.context = context;
            entities = context.Set<Order>();
        }

        public List<Order> GetAllOrders()
        {
            return entities
                .Include(z => z.User)
                .Include(z => z.Movies)
                .Include("Movies.SelectedMovie")
                .ToListAsync().Result;
        }

       
        public Order GetOrderDetails(BaseEntity model)
        {
            return entities
               .Include(z => z.User)
               .Include(z => z.Movies)
               .Include("Movies.SelectedMovie")
               .SingleOrDefaultAsync(z => z.Id == model.Id).Result;
        }

       
    }
}
