using H3CinemaBooking.Repository.Data;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using H3CinemaBooking.Repository.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Repositories
{
        public class BookingRepository : IBookingRepository
        {
            private readonly Dbcontext context;
            private readonly IPropertyValidationService validationService;

        public BookingRepository(Dbcontext context, IPropertyValidationService validationService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        }

        public Booking Create(Booking booking)
        {
            string[] propertiesToSkip = { "BookingSeats", "Show", "userDetail", "BookingID" };

            if (!validationService.ValidateProperties(booking, propertiesToSkip))
            {
                throw new InvalidOperationException("Invalid booking data.");
            }

            context.Bookings.Add(booking);
            context.SaveChanges();
            return booking;
        }

        public Booking GetLatestBookingForUser(int userId)
        {
            var booking = context.Bookings
                .Where(b => b.UserDetailID == userId)
                .Include(b => b.BookingSeats)
                .Include(b => b.Show)
                .OrderByDescending(b => b.BookingID)
                .FirstOrDefault();
            return booking;
        }

        public List<BookingSeat> GetBookingSeatsFromBookingId(int bookingId)
        {
            var seats = context.BookingSeats
                .Where(bs => bs.BookingID == bookingId)
                .ToList();

            return seats;
        }

        public void DeleteBookingSeat(int bookingSeatId)
        {
            var bookingSeat = context.BookingSeats.FirstOrDefault(bs => bs.BookingSeatID == bookingSeatId);
            if (bookingSeat != null)
            {
                context.BookingSeats.Remove(bookingSeat);
                context.SaveChanges();
            }
        }

        public Booking GetById(int Id)
            {
                var result = context.Bookings.FirstOrDefault(c => c.BookingID == Id);
                return result;
            }

            //TODO: Get All Seat
            public List<Booking> GetAll()
            {
                var result = context.Bookings.ToList();
                return result;
            }

        public bool DeleteByID(int Id)
        {
            var booking = context.Bookings.Include(b => b.BookingSeats).FirstOrDefault(c => c.BookingID == Id);
            if (booking != null)
            {
                context.Bookings.Remove(booking);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<BookingSeat> GetBookingSeatsByShowId(int showId)
        {
            // First, get all BookingIDs for the specified showId
            var bookingIds = context.Bookings
                                    .Where(b => b.ShowID == showId)
                                    .Select(b => b.BookingID)
                                    .ToList(); // This will execute the query and return a List<int>

            // Throw an exception if no bookings are found for the given showId
            //if (bookingIds.Count == 0)
            //{
            //    throw new InvalidOperationException($"No bookings found for show ID {showId}");
            //}

            // Then, filter BookingSeats by the obtained BookingIDs
            var result = context.BookingSeats
                                .Where(bs => bookingIds.Contains(bs.BookingID))
                                .Include(bs => bs.Booking)
                                .ToList();

            return result;
        }

        public List<BookingSeat> CreateBookingSeat(Booking booking)
        {
            List<BookingSeat> bookingSeatList = new List<BookingSeat>();

            // Check if BookingSeats is not null and not empty
            if (booking.BookingSeats != null && booking.BookingSeats.Any())
            {
                foreach (var bookingSeat in booking.BookingSeats)
                {
                    // Use validation service to check each bookingSeat, except the BookingSeatID
                    if (!validationService.ValidateProperties(bookingSeat, new string[] { "BookingSeatID", "BookingID", "Booking", "Seat" }))
                    {
                        throw new InvalidOperationException("Invalid booking seat data.");
                    }
                        context.BookingSeats.Add(bookingSeat);
                        bookingSeatList.Add(bookingSeat);
                }
                context.SaveChanges();  // Save all changes at once
                return bookingSeatList;
            }
            else
            {
                throw new InvalidOperationException("Booking must have at least one seat.");
            }
        }

        public void UpdateBookingSeat(BookingSeat bookingSeat)
        {
            context.BookingSeats.Update(bookingSeat);
            context.SaveChanges();
        }

    }
}