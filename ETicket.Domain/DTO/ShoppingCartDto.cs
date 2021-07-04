using ETicket.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<MovieInShoppingCart> Movies { get; set; }

        public double TotalPrice { get; set; }

    }
}
