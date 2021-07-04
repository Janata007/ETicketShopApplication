using ETicket.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Services.Interface
{
    public interface IShoppingCartService 
    {
        ShoppingCartDto getShoppingCartInfo(string userId);
        bool deleteMovieFromShoppingCart(string userId, Guid id);
        bool orderNow(string userId);
    }
}
