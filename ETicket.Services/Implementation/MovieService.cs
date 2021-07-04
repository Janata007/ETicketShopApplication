using ETicket.Domain.DomainModels;
using ETicket.Domain.DTO;
using ETicket.Repository.Interface;
using ETicket.Services.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.Services.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<Movie> _movieRepository;
        private readonly IRepository<MovieInShoppingCart> _movieInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<MovieService> _logger;

        public MovieService(IRepository<Movie> movieRepository, ILogger<MovieService> logger , IRepository<MovieInShoppingCart> movieInShoppingCartRepository, IUserRepository userRepository ) {

            _movieRepository = movieRepository;
            _userRepository = userRepository;
            _movieInShoppingCartRepository = movieInShoppingCartRepository;
            _logger = logger;
        
        }

        public bool AddToShoppingCart(AddToShoppingCartDto item, string userID)
        {

            var user = this._userRepository.Get(userID);

            var userShoppingCart = user.UserCart;

            if (item.MovieId != null && userShoppingCart != null)
            {

                var movie = this.GetDetailsForMovie(item.MovieId);

                if (movie != null)
                {

                    MovieInShoppingCart itemToAdd = new MovieInShoppingCart
                    {
                        Id = Guid.NewGuid(),
                        Movie = movie,
                        MovieId = movie.Id,
                        ShoppingCart = userShoppingCart,
                        ShoppingCartId = userShoppingCart.Id,
                        Quantity = item.TicketQuantity
                    };

                    this._movieInShoppingCartRepository.Insert(itemToAdd);
                    _logger.LogInformation(" Movie was added to shopping cart :D");
                    return true;
                }

                return false;
            }
            _logger.LogInformation(" :( MovieId or UserShoppigCart may be unavailable");
            return false;
        }

        public void CreateNewMovie(Movie m)
        {
            
            this._movieRepository.Insert(m);
        }

        public void DeleteMovie(Guid id)
        {
            var movie = this.GetDetailsForMovie(id);
            this._movieRepository.Delete(movie);
        }

        public List<Movie> GetAllMovies()
        {
          return  this._movieRepository.GetAll().ToList();
        }

        public Movie GetDetailsForMovie(Guid? id)
        {
            return this._movieRepository.Get(id);
        }

        public AddToShoppingCartDto GetShoppingCartInfo(Guid? id)
        {
            var movie = this.GetDetailsForMovie(id);
            AddToShoppingCartDto model = new AddToShoppingCartDto
            {
                SelectedMovie = movie,
                MovieId = movie.Id,
                TicketQuantity = 1
            };

            return model;
        }

        public void UpdateExistingMovie(Movie m)
        {
            this._movieRepository.Update(m);
        }
    }
}
