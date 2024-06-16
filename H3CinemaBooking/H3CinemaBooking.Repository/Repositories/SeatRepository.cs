using H3CinemaBooking.Repository.Data;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Repositories
{
        public class SeatRepository : ISeatRepository
        {
            private readonly Dbcontext context;
            private readonly IPropertyValidationService validationService;

        public SeatRepository(Dbcontext _context, IPropertyValidationService _validationService)
            {
                context = _context;
                validationService = _validationService;
        }

            public Seat Create(Seat seat)
            {
            if (!validationService.ValidateProperties(seat, new string[] { "SeatID", "CinemaHall", "BookingSeats" }))
            {
                throw new ArgumentException("Invalid seat properties.");
            }
                context.Seats.Add(seat);
                context.SaveChanges();
                return seat;
            }


            public IEnumerable<Seat> CreateBulk(IEnumerable<Seat> seats)
            {
                foreach (var seat in seats) 
                {
                    if (!validationService.ValidateProperties(seat, new string[] { "SeatID", "CinemaHall", "BookingSeats" }))
                    {
                        throw new ArgumentException("Invalid seats properties.");
                    }
                }
                
                context.Seats.AddRange(seats);
                context.SaveChanges();
                return seats;
            }

            public Seat GetById(int Id)
            {
                var result = context.Seats.FirstOrDefault(c => c.SeatID == Id);
                return result;
            }

            //TODO: Get All Seat
            public List<Seat> GetAll()
            {
                var result = context.Seats.ToList();
                return result;
            }

            public List<Seat> GetAllSeatsFromHall(int hallID)
            {
                var result = context.Seats.Where(s => s.HallID == hallID).ToList();
                return result;
            }
            public bool DeleteByID(int Id)
            {
                var seat = context.Seats.FirstOrDefault(c => c.SeatID == Id);
                if (seat != null)
                {
                    context.Remove(seat);
                    context.SaveChanges();
                return true;
                }
                return false;
            }

            public Seat UpdateByID(int id, Seat seat)
            {
                var existingSeat = context.Seats.FirstOrDefault(c => c.SeatID == id);
                if (existingSeat == null)
                {
                    throw new ArgumentException("Seat not found.");
                }

                if (!validationService.ValidateProperties(seat, new string[] { "SeatID", "CinemaHall", "BookingSeats" }))
                {
                    throw new ArgumentException("Invalid seat properties.");
                }

                existingSeat.HallID = seat.HallID;
                existingSeat.SeatNumber = seat.SeatNumber;
                existingSeat.SeatRow = seat.SeatRow;

                context.SaveChanges();
                return existingSeat;
            }
    }
    }