using ETicket.Domain.DomainModels;
using ETicket.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Services.Interface
{
   public  interface IMovieService
    {
        List<Movie> GetAllMovies();

        Movie GetDetailsForMovie(Guid? id);

        void CreateNewMovie(Movie m);

        void UpdateExistingMovie(Movie m);

        AddToShoppingCartDto GetShoppingCartInfo(Guid? id);

        void DeleteMovie(Guid id);

        bool AddToShoppingCart(AddToShoppingCartDto item, string userID);

    }
}
