using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain.DomainModels
{
    public class MovieInOrder : BaseEntity
    {

        public Guid MovieId { get; set; }

        public Movie SelectedMovie { get; set; }

        public Guid OrderId { get; set; }

        public Order UserOrder { get; set; }
        public int Quantity { get; set; }
    }
}
