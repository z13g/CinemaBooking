using H3CinemaBooking.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Interfaces
{
    public interface IMovieRepository
    {
        Movie Create(Movie movie);
        Movie GetById(int Id);
        List<Movie> GetAll();
        void UpdateByID(int Id, Movie updatedMovie);
        bool DeleteByID(int Id);
        Movie CreateComplex(Movie movie, List<string>genreNames);
    }
}
