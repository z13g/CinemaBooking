using H3CinemaBooking.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Interfaces
{
    public interface IGenreRepository
    {
        Genre Create(Genre Genre);
        Genre GetById(int Id);
        List<Genre> GetAll();
        bool DeleteGenreByID(int Id);
        void UpdateByID(int Id, Genre updatedGenre);
    }
}
