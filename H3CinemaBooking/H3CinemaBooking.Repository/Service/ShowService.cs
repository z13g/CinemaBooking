using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using H3CinemaBooking.Repository.Models.DTO;
using H3CinemaBooking.Repository.Models.DTO_s;
using H3CinemaBooking.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Service
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _showRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IGenericRepository<CinemaHall> _cinemaHallRepository;
        private readonly IGenericRepository<Cinema> _cinemaRepository;

        public ShowService(
            IShowRepository showRepository, 
            IMovieRepository movieRepository, 
            ISeatRepository seatRepository, 
            IBookingRepository bookingRepository, 
            IGenericRepository<CinemaHall> cinemaHallRepository, 
            IGenericRepository<Cinema> cinemaRepository
        )
        {
            _showRepository = showRepository;
            _movieRepository = movieRepository;
            _seatRepository = seatRepository;
            _bookingRepository = bookingRepository;
            _cinemaHallRepository = cinemaHallRepository;
            _cinemaRepository = cinemaRepository;
        }

        private CinemaHall GetCinemaHallObjectFromHallId(int hallID)
        {
            //Get CinemaHall Object from hallID
            return _cinemaHallRepository.GetById(hallID);
        }

        private Cinema GetCinemaObjectFromCinemaID(int cinemaID)
        {
            return _cinemaRepository.GetById(cinemaID);
        } 

        private Movie GetMovieObjectFromMovieId(int movieID)
        {
            return _movieRepository.GetById(movieID);
        }

        public BookShow SetBookShowObjekt(int showId)
        {
            BookShow bookShow = new BookShow();
            var show = _showRepository.GetById(showId);
            if (show != null)
            {
                bookShow.Price = show.Price;
                //Get CinemaHall Object
                var cinemaHall = GetCinemaHallObjectFromHallId(show.HallID);
                bookShow.HallName = cinemaHall.HallName;

                //Get Cinema Object
                var cinema = GetCinemaObjectFromCinemaID(cinemaHall.CinemaID);
                bookShow.CinemaName = cinema.Name;

                var movie = GetMovieObjectFromMovieId(show.MovieID);
                bookShow.Movie = movie;

                bookShow.ShowDateTime = show.ShowDateTime;
                
                var seats = _seatRepository.GetAllSeatsFromHall(show.HallID);
                var bookedSeats = _bookingRepository.GetBookingSeatsByShowId(showId);
                bookShow.showId = showId;

                // Tildel status til hver sæde
                foreach (var seat in seats)
                {
                    var seatInfo = new SeatDTO
                    {
                        HallID = seat.HallID,
                        SeatID = seat.SeatID,
                        SeatNumber = seat.SeatNumber,
                        SeatRow = seat.SeatRow,
                        SeatStatus = "Available"
                    };

                    var bookingSeat = bookedSeats.FirstOrDefault(bs => bs.SeatID == seat.SeatID);
                    if (bookingSeat != null)
                    {
                        //Check with userdetails of the current logged in user is the one who has reversed, 
                        // if thats true then it has to change seatstatus to booked
                        SeatStatus status = bookingSeat.Status;
                        string statusString = status.ToString();
                        seatInfo.SeatStatus = statusString;
                    }

                    bookShow.Seats.Add(seatInfo);
                }
                //bookShow.Seats = seats;


            }
            return bookShow;
        }

        public Dictionary<string, Dictionary<DateTime, List<ShowDetailsDTO>>> GetFilteredShowsByCinemaAndDate(int areaId, int movieId)
        {
            var today = DateTime.Today;
            var endDay = today.AddDays(10);

            // Retrieve all shows that are within the next 10 days and match the movie ID
            var shows = _showRepository.GetAll()
                                       .Where(s => s.ShowDateTime.Date >= today && s.ShowDateTime.Date <= endDay && s.MovieID == movieId)
                                       .ToList();

            var result = new Dictionary<string, Dictionary<DateTime, List<ShowDetailsDTO>>>();

            foreach (var show in shows)
            {
                var cinemaHall = _cinemaHallRepository.GetById(show.HallID);
                var cinema = _cinemaRepository.GetById(cinemaHall.CinemaID);

                // Filter cinemas by the specified area
                if (cinema.AreaID != areaId)
                    continue;

                var cinemaName = cinema.Name;

                if (!result.ContainsKey(cinemaName))
                {
                    result[cinemaName] = new Dictionary<DateTime, List<ShowDetailsDTO>>();
                }

                for (int i = 0; i <= 10; i++)
                {
                    var date = today.AddDays(i);
                    if (!result[cinemaName].ContainsKey(date))
                    {
                        result[cinemaName][date] = new List<ShowDetailsDTO>();
                    }

                    if (show.ShowDateTime.Date == date)
                    {
                        var showDetails = new ShowDetailsDTO
                        {
                            ShowId = show.ShowID,
                            CinemaID = cinema.CinemaID,
                            CinemaName = cinemaName,
                            HallName = cinemaHall.HallName,
                            ShowDateTime = show.ShowDateTime,
                            MovieTitle = _movieRepository.GetById(show.MovieID).Title
                        };

                        result[cinemaName][date].Add(showDetails);
                    }
                }
            }

            return result;
        }



    }
}
