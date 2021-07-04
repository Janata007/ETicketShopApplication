using ETicket.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<ETicketApplicationUser> GetAll();

        ETicketApplicationUser Get(string? id);

        void Insert(ETicketApplicationUser entity);

        void Update(ETicketApplicationUser entity);

        void Delete(ETicketApplicationUser entity);


    }
}
