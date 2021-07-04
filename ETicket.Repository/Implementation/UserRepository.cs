using ETicket.Domain.Identity;
using ETicket.Repository.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<ETicketApplicationUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<ETicketApplicationUser>();
        }
        public IEnumerable<ETicketApplicationUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public ETicketApplicationUser Get(string? id)
        {
            return entities
                .Include(z => z.UserCart)
                .Include("UserCart.MoviesInShoppingCarts")
                .Include("UserCart.MoviesInShoppingCarts.Movie")
                .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(ETicketApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(ETicketApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(ETicketApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
