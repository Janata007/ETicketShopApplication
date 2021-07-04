using ETicket.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Services.Interface
{
    public interface IOrderService
    {

        List<Order> GetAllOrders();
        Order GetOrderDetails(BaseEntity model);
    }
}
