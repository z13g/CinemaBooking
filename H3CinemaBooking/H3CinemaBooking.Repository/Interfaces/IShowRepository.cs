using H3CinemaBooking.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Interfaces
{
    public interface IShowRepository
    {
        Show Create(Show show);
        Show GetById(int Id);
        List<Show> GetAll();
        void UpdateByID(int Id, Show updatedShow);
        bool DeleteByID(int id);

        double GetShowPrice(int showId);
    }
}
