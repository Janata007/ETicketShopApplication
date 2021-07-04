using ETicket.Domain.DomainModels;
using ETicket.Domain.DTO;
using ETicket.Repository.Interface;
using ETicket.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.Services.Implementation
{
   public  class ShoppingCartService : IShoppingCartService
    {

        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<MovieInOrder> _movieInOrderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<EmailMessage> _mailRepository;

        public ShoppingCartService(IRepository<EmailMessage> mailRepository, IRepository<ShoppingCart> shoppingCartRepository, IRepository<MovieInOrder> movieInOrderRepository, IRepository<Order> orderRepository, IUserRepository userRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _movieInOrderRepository = movieInOrderRepository;
            _mailRepository = mailRepository;
        }

        
        public bool deleteMovieFromShoppingCart(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
              

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart.MoviesInShoppingCarts.Where(z => z.MovieId.Equals(id)).FirstOrDefault();

                userShoppingCart.MoviesInShoppingCarts.Remove(itemToDelete);

                this._shoppingCartRepository.Update(userShoppingCart);

                return true;
            }

            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = this._userRepository.Get(userId);

            var userShoppingCart = loggedInUser.UserCart;

            var AllMovies = userShoppingCart.MoviesInShoppingCarts.ToList();

            var allMoviePrice = AllMovies.Select(z => new
            {
                MoviePrice = z.Movie.MoviePrice,
                Quanitity = z.Quantity
            }).ToList();

            var totalPrice = 0;


            foreach (var item in allMoviePrice)
            {
                totalPrice += item.Quanitity * item.MoviePrice;
            }


            ShoppingCartDto scDto = new ShoppingCartDto
            {
                Movies = AllMovies,
                TotalPrice = totalPrice
            };


            return scDto;

        }

        public bool orderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                EmailMessage mail = new EmailMessage();
                mail.MailTo = loggedInUser.Email;
                mail.Subject = "Successfully created order";
                mail.Status = false;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId
                };

                this._orderRepository.Insert(order);

                List<MovieInOrder> movieInOrders = new List<MovieInOrder>();

                var result = userShoppingCart.MoviesInShoppingCarts.Select(z => new MovieInOrder
                {
                    Id = Guid.NewGuid(),
                    MovieId = z.Movie.Id,
                    SelectedMovie = z.Movie,
                    OrderId = order.Id,
                    UserOrder = order,
                    Quantity = z.Quantity
                }).ToList();

                StringBuilder sb = new StringBuilder();

                var totalPrice = 0;

                sb.AppendLine("Your order is completed. The order has: ");

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    totalPrice += item.Quantity * item.SelectedMovie.MoviePrice;

                    sb.AppendLine(i.ToString() + ". " + item.SelectedMovie.MovieName + " with price of: " + item.SelectedMovie.MoviePrice + " and quantity of: " + item.Quantity);
                }

                sb.AppendLine("Total price: " + totalPrice.ToString());
                 

                mail.Content = sb.ToString();


                movieInOrders.AddRange(result);

                foreach (var item in movieInOrders)
                {
                    this._movieInOrderRepository.Insert(item);
                }

                loggedInUser.UserCart.MoviesInShoppingCarts.Clear();

                this._userRepository.Update(loggedInUser);
                this._mailRepository.Insert(mail);

                return true;
            }
            return false;
        }
    }
}
