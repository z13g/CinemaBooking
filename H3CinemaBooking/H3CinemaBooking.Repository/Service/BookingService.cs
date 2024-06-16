using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using H3CinemaBooking.Repository.Models.DTO;
using H3CinemaBooking.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IShowRepository _showRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IGenericRepository<CinemaHall> _cinemaHallRepository;
        private readonly IGenericRepository<Cinema> _cinemaRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IShowRepository showRepository,
            IMovieRepository movieRepository,
            IGenericRepository<CinemaHall> cinemaHallRepository,
            IGenericRepository<Cinema> cinemaRepository
            )
        {
            _bookingRepository = bookingRepository;
            _showRepository = showRepository;
            _movieRepository = movieRepository;
            _cinemaHallRepository = cinemaHallRepository;
            _cinemaRepository = cinemaRepository;
        }

        public BookingDTO GetLatestBookingForUser(int userId)
        {
            BookingDTO bookingDTO = new BookingDTO();
            var bookingObj = _bookingRepository.GetLatestBookingForUser(userId);
            if (bookingObj != null)
            {
                bookingDTO.NumberOfSeats = bookingObj.NumberOfSeats;
                bookingDTO.Price = bookingObj.Price;
                bookingDTO.BookingID = bookingObj.BookingID;
                bookingDTO.ShowID = bookingObj.ShowID;

                // Include BookingSeats and Show data
                var bookingSeats = _bookingRepository.GetBookingSeatsFromBookingId(bookingObj.BookingID);
                if (bookingSeats != null)
                {
                    bookingDTO.BookingSeats = bookingSeats;
                }
                var show = _showRepository.GetById(bookingObj.ShowID);
                if (show != null)
                {
                    bookingDTO.Show = show;
                    var movie = _movieRepository.GetById(show.MovieID);
                    if (movie != null)
                    {
                        bookingDTO.Movie = movie;
                    }
                    //Get CinemaObjekt
                    var cinemaHall = _cinemaHallRepository.GetById(show.HallID);
                    if (cinemaHall != null)
                    {
                        var cinema = _cinemaRepository.GetById(cinemaHall.CinemaID);
                        if (cinema != null)
                        {
                            bookingDTO.Cinema = cinema;
                        }
                    }

                }
            }
            return bookingDTO;
        }

        public ReserveSeatResultDTO ReserveSeats(ReserveSeatDTO reserveSeat)
        {
            var result = new ReserveSeatResultDTO();
            var existingBookingSeats = _bookingRepository.GetBookingSeatsByShowId(reserveSeat.ShowID);

            // Hent prisen fra showet
            double showPrice = _showRepository.GetShowPrice(reserveSeat.ShowID);

            // Initialize a new booking
            Booking booking = new Booking
            {
                ShowID = reserveSeat.ShowID,
                UserDetailID = reserveSeat.UserID,
                BookingSeats = new List<BookingSeat>()
            };

            int reservedSeatCount = 0;
            bool seatsChanged = false;

            foreach (var seatDTO in reserveSeat.SeatList)
            {
                var existingSeat = existingBookingSeats.FirstOrDefault(bs => bs.SeatID == seatDTO.SeatID);

                if (existingSeat != null)
                {
                    if (existingSeat.Booking.UserDetailID == reserveSeat.UserID)
                    {
                        if (Enum.TryParse<SeatStatus>(seatDTO.SeatStatus, true, out SeatStatus newStatus))
                        {
                            if (newStatus == SeatStatus.Available)
                            {
                                // Delete the existing booking seat
                                _bookingRepository.DeleteBookingSeat(existingSeat.BookingSeatID);
                                result.SeatStatuses.Add(new SeatReservationStatusDTO { SeatID = seatDTO.SeatID, SeatStatus = "Deleted" });
                                seatsChanged = true;
                            }
                            else if (existingSeat.Status != newStatus)
                            {
                                existingSeat.Status = newStatus;
                                _bookingRepository.UpdateBookingSeat(existingSeat);
                                result.SeatStatuses.Add(new SeatReservationStatusDTO { SeatID = seatDTO.SeatID, SeatStatus = "Updated" });
                                seatsChanged = true;
                            }
                            else
                            {
                                result.SeatStatuses.Add(new SeatReservationStatusDTO { SeatID = seatDTO.SeatID, SeatStatus = "NoChange" });
                            }
                        }
                        else
                        {
                            result.SeatStatuses.Add(new SeatReservationStatusDTO { SeatID = seatDTO.SeatID, SeatStatus = "InvalidStatus" });
                        }
                    }
                    else
                    {
                        result.SeatStatuses.Add(new SeatReservationStatusDTO { SeatID = seatDTO.SeatID, SeatStatus = "AlreadyBooked" });
                    }
                }
                else
                {
                    if (seatDTO.SeatStatus == "Reserved" || seatDTO.SeatStatus == "Booked")
                    {
                        BookingSeat newBookingSeat = new BookingSeat
                        {
                            SeatID = seatDTO.SeatID,
                            Booking = booking,
                            Status = Enum.Parse<SeatStatus>(seatDTO.SeatStatus)
                        };

                        booking.BookingSeats.Add(newBookingSeat);
                        result.SeatStatuses.Add(new SeatReservationStatusDTO { SeatID = seatDTO.SeatID, SeatStatus = "Reserved" });
                        reservedSeatCount++;
                        seatsChanged = true;
                    }
                }
            }

            if (seatsChanged)
            {
                if (reservedSeatCount > 0)
                {
                    booking.NumberOfSeats = reservedSeatCount;
                    booking.Price = showPrice * reservedSeatCount;
                    _bookingRepository.Create(booking);
                }
                result.Success = true;
                result.Message = "Seats reserved or updated successfully.";
            }
            else
            {
                result.Success = false;
                result.Message = "No seats reserved.";
            }

            return result;
        }





        //public void ReserveSeats(ReserveSeatDTO reserveSeat)
        //{
        //    // Tjek om nogen af sæderne allerede er reserveret
        //    var existingBookings = _bookingRepository.GetBookingSeatsByShowId(reserveSeat.ShowID);
        //    var seatsToReserve = reserveSeat.SeatList;

        //    foreach (var seat in seatsToReserve)
        //    {
        //        var existingBookingSeat = existingBookings.FirstOrDefault(bs => bs.SeatID == seat.SeatID);

        //        if (existingBookingSeat != null)
        //        {
        //            if (existingBookingSeat.Booking.UserDetailID == reserveSeat.UserID)
        //            {
        //                // Hvis sædet allerede er reserveret af samme bruger, tjek om status skal opdateres
        //                if (existingBookingSeat.Status.ToString() != seat.SeatStatus)
        //                {
        //                    existingBookingSeat.Status = Enum.Parse<SeatStatus>(seat.SeatStatus, true);
        //                    _bookingRepository.UpdateBookingSeat(existingBookingSeat);
        //                }
        //            }
        //            else
        //            {
        //                // Hvis sædet allerede er reserveret af en anden bruger, kast en undtagelse
        //                throw new InvalidOperationException($"Seat {seat.SeatNumber} in row {seat.SeatRow} is already reserved.");
        //            }
        //        }
        //    }

        //    // Hvis der ikke er nogen konflikter, opret ny booking og book seats
        //    Booking booking = new Booking
        //    {
        //        ShowID = reserveSeat.ShowID,
        //        UserDetailID = reserveSeat.UserID,
        //        NumberOfSeats = reserveSeat.SeatList.Count,
        //        BookingSeats = new List<BookingSeat>(),
        //        Price = reserveSeat.Price * reserveSeat.SeatList.Count
        //    };

        //    var bookingCreated = _bookingRepository.Create(booking);

        //    foreach (var seat in seatsToReserve)
        //    {
        //        BookingSeat bookingSeat = new BookingSeat
        //        {
        //            SeatID = seat.SeatID,
        //            BookingID = bookingCreated.BookingID,
        //            Status = Enum.Parse<SeatStatus>(seat.SeatStatus, true)
        //        };

        //        booking.BookingSeats.Add(bookingSeat);
        //    }

        //    _bookingRepository.CreateBookingSeat(booking);
        //}
    }
}
