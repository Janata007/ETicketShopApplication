using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace ETicket.Domain.DomainModels
{
    public class Movie : BaseEntity
    {
        [Required]
        public string MovieName { get; set; }
        [Required]
        public string MovieImage { get; set; }
        [Required]
        public string MovieDescription { get; set; }
        [Required]
        public int MoviePrice { get; set; }
        [Required]
        public int  Rating { get; set; }
        [Required]
        public DateTime Date { get; set; }
       

        public virtual ICollection<MovieInShoppingCart> MoviesInShoppingCarts { get; set; }
        public virtual IEnumerable<MovieInOrder> Orders { get; set; }
    }
}
