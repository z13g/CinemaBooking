using H3CinemaBooking.Repository.Data;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly Dbcontext context;
        private readonly IPropertyValidationService validationService;

        public GenreRepository(Dbcontext _context, IPropertyValidationService _validationService)
        {
            context = _context;
            validationService = _validationService;
        }
        public Genre Create(Genre Genre)
        {
            if (!validationService.ValidateProperties(Genre, new string[] { "GenreID", "Movies" }))
            {
                throw new ArgumentException("Invalid genre properties.");
            }

            context.Genres.Add(Genre);
            context.SaveChanges();
            return Genre;
        }
        public Genre GetById(int Id)
        {
            var result = context.Genres.FirstOrDefault(c => c.GenreID == Id);
            return result;
        }
        public List<Genre> GetAll()
        {
            var result = context.Genres.ToList();
            return result;
        }

        public void UpdateByID(int Id, Genre updatedGenre)
        {
            if (!validationService.ValidateProperties(updatedGenre, new string[] { "GenreID", "Movies" }))
            {
                throw new ArgumentException("Invalid genre properties.");
            }

            var genre = context.Genres.FirstOrDefault(g => g.GenreID == Id);

            if (genre != null)
            {
                genre.GenreName = updatedGenre.GenreName;

                context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException($"No genre found with ID {Id}.");
            }
        }

        public bool DeleteGenreByID(int Id)
        {
            var genre = context.Genres.FirstOrDefault(c => c.GenreID == Id);
            if (genre != null)
            {
                context.Remove(genre);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
