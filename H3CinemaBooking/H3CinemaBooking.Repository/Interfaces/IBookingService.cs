using H3CinemaBooking.Repository.Models;
using H3CinemaBooking.Repository.Models.DTO;
using H3CinemaBooking.Repository.Models.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Interfaces
{
    public interface IBookingService
    {
        ReserveSeatResultDTO ReserveSeats(ReserveSeatDTO reserveSeat);
        BookingDTO GetLatestBookingForUser(int userId);
    }
}
