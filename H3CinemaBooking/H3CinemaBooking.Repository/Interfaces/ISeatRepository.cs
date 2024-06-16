using H3CinemaBooking.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Interfaces
{
    public interface ISeatRepository
    {
        Seat Create(Seat seat);
        public IEnumerable<Seat> CreateBulk(IEnumerable<Seat> seats);
        Seat GetById(int id);
        List<Seat> GetAll();
        bool DeleteByID(int id);
        Seat UpdateByID(int id, Seat seat);

        List<Seat> GetAllSeatsFromHall(int hallID);
    }
}
