
using ETicket.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain.DomainModels
{
    public class ShoppingCart: BaseEntity
    {
        
        public string OwnerId { get; set; } //nadvoresen kluc kon app user

        public  ETicketApplicationUser Owner { get; set; }

        public virtual ICollection<MovieInShoppingCart> MoviesInShoppingCarts { get; set; }

    }
}
