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
    public interface IShowService
    {
        BookShow SetBookShowObjekt(int showId);
        public Dictionary<string, Dictionary<DateTime, List<ShowDetailsDTO>>> GetFilteredShowsByCinemaAndDate(int areaId, int movieId);
    }
}
