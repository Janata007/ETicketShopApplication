using ETicket.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain.DTO
{ 
    public class AddToShoppingCartDto
    {
        public Movie SelectedMovie { get; set; }
        public Guid MovieId { get; set; }
        public int TicketQuantity { get; set; }
    }
}
