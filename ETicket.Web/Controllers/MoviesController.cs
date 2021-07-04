using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ETicket.Domain.Identity;
using ETicket.Domain.DomainModels;
using ETicket.Domain.DTO;
using System.Data.Entity;
using ETicket.Repository;
using ETicket.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;
using System.Net.Http;
using System.IO;

namespace ETicket.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;

        //public object AddTosShoppingCartDto { get; private set; }

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
            
        }

        // GET: Movies
        public IActionResult Index()
        {
            var allMovies = this._movieService.GetAllMovies();

            return View(allMovies);
        }
        
         public IActionResult AddMovieToCard(Guid? id)
        {
            var model = this._movieService.GetShoppingCartInfo(id);

            return View(model);
        }       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMovieToCard([Bind("MovieId","TicketQuantity")] AddToShoppingCartDto item)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

           var result = this._movieService.AddToShoppingCart(item, userId);

            if (result) {

                return RedirectToAction("Index", "Movies");

            }
          
            return  View();

        }

        // GET: Movies/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = this._movieService.GetDetailsForMovie(id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,MovieName,MovieImage,MovieDescription,MoviePrice,Rating,Date")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                this._movieService.CreateNewMovie(movie);

                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = this._movieService.GetDetailsForMovie(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,MovieName,MovieImage,MovieDescription,MoviePrice,Rating,Date")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._movieService.UpdateExistingMovie(movie);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = this._movieService.GetDetailsForMovie(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this._movieService.DeleteMovie(id);
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(Guid id)
        {
            return this._movieService.GetDetailsForMovie(id) != null;
        }


        public FileContentResult ExportAllTickets()
        {

            string fileName = "Tickets.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Tickets");
                worksheet.Cell(1, 1).Value = "TicketName";
                worksheet.Cell(1, 2).Value = "Genre";
                worksheet.Cell(1, 3).Value = "Date";
                worksheet.Cell(1, 4).Value = "Time";
                worksheet.Cell(1, 5).Value = "Price";
                worksheet.Cell(1, 6).Value = "Rating";

                //string zanr =  MyAction();
                List<Movie> result = _movieService.GetAllMovies();
                //var s = MyAction();
                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.MovieName.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.MovieDescription.ToString();
          
                    worksheet.Cell(i + 1, 5).Value = item.MoviePrice.ToString();
                    worksheet.Cell(i + 1, 6).Value = item.Rating.ToString();


                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
            return null;
        }


        public FileContentResult ExportTicketGenre(string valueINeed)
        {
            string fileName = "Tickets.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Tickets");
                worksheet.Cell(1, 1).Value = "TicketName";
                worksheet.Cell(1, 2).Value = "Genre";
                worksheet.Cell(1, 3).Value = "Date";
                worksheet.Cell(1, 4).Value = "Time";
                worksheet.Cell(1, 5).Value = "Price";
                worksheet.Cell(1, 6).Value = "Rating";

                //string zanr =  MyAction();
                List<Movie> result = _movieService.GetAllMovies();
                //var s = MyAction();
                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];
                    if (valueINeed == item.MovieDescription.ToString())
                    {
                        worksheet.Cell(i + 1, 1).Value = item.MovieName.ToString();
                        worksheet.Cell(i + 1, 2).Value = item.MovieDescription.ToString();
                 
                        worksheet.Cell(i + 1, 5).Value = item.MoviePrice.ToString();
                        worksheet.Cell(i + 1, 6).Value = item.Rating.ToString();
                    }

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
            return null;
        }





    }
}
