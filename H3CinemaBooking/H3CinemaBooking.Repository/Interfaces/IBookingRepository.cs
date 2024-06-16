using H3CinemaBooking.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Interfaces
{
    public interface IBookingRepository
    {
        Booking Create(Booking booking);
        Booking GetById(int id);
        List<Booking> GetAll();
        bool DeleteByID(int id);
        List<BookingSeat> GetBookingSeatsByShowId(int showId);
        List<BookingSeat> CreateBookingSeat(Booking booking);
        void UpdateBookingSeat(BookingSeat bookingSeat);
        void DeleteBookingSeat(int bookingSeatId);
        Booking GetLatestBookingForUser(int userId);
        List<BookingSeat> GetBookingSeatsFromBookingId(int bookingId);
    }
}
